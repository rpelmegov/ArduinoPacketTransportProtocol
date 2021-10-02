#ifndef __List_h__
#define __List_h__

template <class T>
class TPDataListItem
{
public:
	TPDataListItem<T>* next;
	T                  value;

	TPDataListItem(T val)
	{
		next  = 0;
		value = val;
	}
};

template <class T>
class List
{
public:
	TPDataListItem<T>* first;
	TPDataListItem<T>* last;
	int Length;

	List()
	{
		Length = 0;
		first  = 0;
		last   = 0;
	}

  void ResetLength()
  {
    first  = 0;
    last   = 0;
    Length = 0;
  }

	void Reset()
	{
		for (TPDataListItem<T>* item = first; item != 0;)
		{
			TPDataListItem<T>* del = item;

			item = item->next;

			delete del;
		}

    ResetLength();
	}

	void Add(T value)
	{
		TPDataListItem<T>* item = new TPDataListItem<T>(value);

		if (Length == 0)
		{
			first = item;
			last  = item;
		}
		else
		{
			last->next = item;
			last       = item;
		}

		Length++;
	}

	void SetValue(int index, T value)
	{
		TPDataListItem<T>* item = first;
    
		int i = 0;
		for (TPDataListItem<T>* item = first; item != 0; item = item->next)
		{
			if (i++ == index)
			{
				item->value = value;
				break;
			}
		}
	}

  T EjectFirst()
  {
    // Запомним данные
    T val = first->value;

    // Скопируем ссылку на первый элемент
    TPDataListItem<T>* del = first;

    // Переназначим первый элемент
    first = first->next;

    // Удалим бывший первый элемент
    delete del;

    // Вернем данные
    return val;
  }
};

#endif

