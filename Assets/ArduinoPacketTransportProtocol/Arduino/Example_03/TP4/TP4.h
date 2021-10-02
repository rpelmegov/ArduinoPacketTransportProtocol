#ifndef __TP4_h__
#define __TP4_h__

#include "List.h"

struct AL
{
  byte  len;
  byte* arr;
};

class TP4
{
public:
	virtual int  Available();
	virtual int  Read();
	virtual int  Peek();
	virtual void Flush();
  virtual void SendBytes(byte* arr, byte len);
  
  byte* RX_packHead;
  int   RX_packHead_ss_Len;
  int   RX_packHead_Len;

  byte* TX_packHead;
  int   TX_packHead_ss_Len;
  int   TX_packHead_Len;
  
  List<AL>*   packBody;
  List<byte>* packBodyReceive;
  int         startIndex = 0;
  
  // Установить стартовую последовательность
  void SetStartSequence(byte* Seq, int Len)
  {
    // Длина заголовка это длина стартовой последовательности Len + сумма байт в теле (2 байта) + число байт в теле (1 байт)
    RX_packHead_ss_Len = Len;
    RX_packHead_Len    = Len + 2 + 1;
    RX_packHead        = new byte[RX_packHead_Len];

    TX_packHead_ss_Len = Len;
    TX_packHead_Len    = Len + 2 + 1;
    TX_packHead        = new byte[TX_packHead_Len];
    
    // Зададим значения
    memcpy(RX_packHead, Seq, Len);
    memcpy(TX_packHead, Seq, Len);
    
    // Создадим список тела пакета
    packBody = new List<AL>();
    
    // Создадим список тела пакета
    packBodyReceive = new List<byte>();
  }

  // Начать формировать новый пакет
  void BeginPack()
  {
    // Сбросим счетчики
    TX_packHead[TX_packHead_Len - 3]  = 0;
    TX_packHead[TX_packHead_Len - 2]  = 0;
    TX_packHead[TX_packHead_Len - 1]  = 0;

    // Переберем данные
    for (TPDataListItem<AL>* item = packBody->first; item != 0;)
    {
      // Освободим память
      delete[] (item->value).arr;
      
      // 
      TPDataListItem<AL>* del = item;

      // 
      item = item->next;

      // 
      delete del;
    }
    
    packBody->ResetLength();
  }

  // Добавить в пакет
  template< class T >
  void PushToPack(T* val, int len = 1)
  {
    // Структуру данных создадим
    AL s;
    
    // Вычислим размер типа
    s.len = sizeof(*val) * len;

    // Выделим память под копию объекта
    s.arr = new byte[s.len];
    
    // Скопируем объект
    memcpy(s.arr, val, s.len);

    // В тело засунем
    packBody->Add(s);

    // Сумму длин прибавим
    TX_packHead[TX_packHead_Len - 1] += s.len;

    // Посчитаем сумму байт
    uint16_t* sum = (uint16_t*)&TX_packHead[TX_packHead_Len - 3];
    for (int i = 0; i < s.len; i++)
    {
      *sum += s.arr[i];
    }
  }

  // Передать пакет
  void SendPack()
  {
    // Передадим шапку
    SendBytes(TX_packHead, TX_packHead_Len);
    
    // Переберем данные
    for (TPDataListItem<AL>* item = packBody->first; item != 0; item = item->next)
    {
      // Передадим заначку
      SendBytes(item->value.arr, item->value.len);
    }
  }

	// Прослушка
  int      progress       = 0;
  int      progress2      = 0;
  byte     controlSum[2];
  uint16_t controlSum2    = 0;
  int      receivePackLen = 0;
	boolean Listen()
	{
    // Получим количество байт в порту
    byte av = Available();
  
    // Если в порту набралось данных
		for (int i = 0; i < av; i++)
		{
      // Заберем байт
      if (ReceiveDataProcessing(Read())) return true;
		}
    
		return false;
	}

  bool ReceiveDataProcessing(byte b)
  {
      // Если прогресса недостаточно
      if (progress < RX_packHead_ss_Len)
      {
        // Заберем первый байт и сравним с заголовком
        if (b == RX_packHead[progress])
        {
          // Нарастим прогресс, если байт подошел
          progress++;
        }
        else
        {
          // Сбросим прогресс, если байт не подошел
          progress = 0;
        }
      }
      // Если мы успешно считали стартовую последовательность
      else
      {
        // Считаем 2 байта контрольной суммы
        if (progress < RX_packHead_ss_Len + 2)
        {
          // Считаем
          controlSum[progress - RX_packHead_ss_Len] = b;

          // Нарастим прогресс
          progress++;
        }
        else
        {
          // Считаем количество байт в пакете
          if (progress < RX_packHead_Len)
          {
            receivePackLen = b;
  
            // Нарастим прогресс
            progress++;

            // Очистим список
            packBodyReceive->Reset();
          }
          else
          {
            // Считываем вонючие байты пока всё не считаем
            if (progress2 < receivePackLen)
            {
              // Посчитаем сумму
              controlSum2 += b;
              
              // Запомним считанный байт
              packBodyReceive->Add(b);
              
              // Нарастим прогресс
              progress2++;

              // Если считывание завершено
              if (progress2 == receivePackLen)
              {
                bool result;
                
                // Сравним контрольные суммы
                result = controlSum2 == *((uint16_t*)&controlSum[0]);

                // Сбросим прогрессы
                progress       = 0;
                progress2      = 0;
                //controlSum[2];
                controlSum2    = 0;
                receivePackLen = 0;
                startIndex     = 0;

                // Вернем результат
                return result;
              }
            }
          }
        }
      }

      return false;
  }
  
	// Деструктор
	void Dispose()
	{
    // Удадим данные
    delete[] RX_packHead;
  
    // Удадим данные
    packBody->Reset();
    delete packBody;
  
    // Удалим данные
    packBodyReceive->Reset();
		delete packBodyReceive;
	}

  // 
  template< class T >
  T GetVal()
  {
    // Создадим переменную
    T val;
    
    // Вычислим размер типа
    int cnt = sizeof(T);

    // Переберем байты переменной
    for (int i = 0; i < cnt; i++)
    {
      *((byte*)&val + i) = packBodyReceive->EjectFirst();
    }

    // Вернем
    return val;
  }
  
  template< class T >
  void GetArr(void* arr, int* len = 0)
  {
    // Преобразуем тип указателя
    T* _arr = arr;
    
    // Получим размер
    int _len = GetVal<byte>();
    
    // Пройдемся по ячейкам
    for (int i = 0; i < _len; i++)
    {
      // Соберем данные
      _arr[i] = GetVal<T>();
    }

    // Данные отдадим если надо
    if (len != 0) *len = _len;
  }
  
  template< class T >
  T* GetArr(int* len = 0)
  {
    // Получим размер
    int _len = GetVal<byte>();

    // 
    // Выделим память
    // 
    T* arr = new T[_len];
    
    // Соберем данные
    for (int i = 0; i < _len; i++)
    {
      // Соберем данные
      arr[i] = GetVal<T>();
    }

    // Данные отдадим
    if (len != 0) *len = _len;
    
    // Вернем ответ
    return arr;
  }
};

#endif

