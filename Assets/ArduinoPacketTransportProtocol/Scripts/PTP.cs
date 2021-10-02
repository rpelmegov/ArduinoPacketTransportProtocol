using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using System.IO.Ports;

public class PTP : MonoBehaviour
{
    // 
    public class DATA
    {
        public byte[] raw        = new byte[0];
        public int    startIndex = 0;

        public List<byte> toSend = new List<byte>();
        
        public bool GetBool()
        {
            var a       = BitConverter.ToBoolean(raw, startIndex);
            startIndex += 1;
            return a;
        }
        public sbyte GetChar()
        {
            var a = ((sbyte)raw[startIndex]);
            startIndex += 1;
            return a;
        }
        public byte GetByte()
        {
            var a       = raw[startIndex];
            startIndex += 1;
            return a;
        }
        public int GetInt()
        {
            var a       = BitConverter.ToInt16(raw, startIndex);
            startIndex += 2;
            return a;
        }
        public int GetUInt()
        {
            var a = BitConverter.ToUInt16(raw, startIndex);
            startIndex += 2;
            return a;
        }
        public int GetLong()
        {
            var a       = BitConverter.ToInt32(raw, startIndex);
            startIndex += 4;
            return a;
        }
        public uint GetULong()
        {
            var a       = BitConverter.ToUInt32(raw, startIndex);
            startIndex += 4;
            return a;
        }
        public float GetFloat()
        {
            var a       = BitConverter.ToSingle(raw, startIndex);
            startIndex += 4;
            return a;
        }
        public double GetDouble()
        {
            var a       = BitConverter.ToSingle(raw, startIndex);
            startIndex += 4;
            return a;
        }
        
        public bool[] GetBool(int len)
        {
            // Создадим массив заданной длины
            var arr = new bool[len];

            for (int i = 0; i < len; i++) arr[i] = GetBool();

            // Вернем массив
            return arr;
        }
        public sbyte[] GetChar(int len)
        {
            // Создадим массив заданной длины
            var arr = new sbyte[len];

            for (int i = 0; i < len; i++) arr[i] = GetChar();

            // Вернем массив
            return arr;
        }
        public byte[] GetByte(int len)
        {
            // Создадим массив заданной длины
            var arr = new byte[len];

            for (int i = 0; i < len; i++) arr[i] = GetByte();

            // Вернем массив
            return arr;
        }
        public int[] GetInt(int len)
        {
            // Создадим массив заданной длины
            var arr = new int[len];

            for (int i = 0; i < len; i++) arr[i] = GetInt();

            // Вернем массив
            return arr;
        }
        public int[] GetUInt(int len)
        {
            // Создадим массив заданной длины
            var arr = new int[len];

            //for (int i = 0; i < len; i++) arr[i] = GetUInt();

            // Вернем массив
            return arr;
        }
        public int[] GetLong(int len)
        {
            // Создадим массив заданной длины
            var arr = new int[len];

            for (int i = 0; i < len; i++) arr[i] = GetLong();

            // Вернем массив
            return arr;
        }
        public uint[] GetULong(int len)
        {
            // Создадим массив заданной длины
            var arr = new uint[len];

            for (int i = 0; i < len; i++) arr[i] = GetULong();

            // Вернем массив
            return arr;
        }
        public float[] GetFloat(int len)
        {
            // Создадим массив заданной длины
            var arr = new float[len];

            for (int i = 0; i < len; i++) arr[i] = GetFloat();

            // Вернем массив
            return arr;
        }
        public double[] GetDouble(int len)
        {
            // Создадим массив заданной длины
            var arr = new double[len];

            for (int i = 0; i < len; i++) arr[i] = GetDouble();

            // Вернем массив
            return arr;
        }

        
        public static T[][] SplitArray<T>(T[] src, int dim1, int dim2)
        {
            // Создадим массив заданной длины
            T[][] arr = new T[dim1][];
            
            // Переберем первое измерение
            for (int i = 0; i < dim1; i++)
            {
                // Выберем второе измерение
                arr[i] = src.Skip(i * dim2).Take(dim2).ToArray();
            }
            
            // Вернем массив
            return arr;
        }
        public static object SplitArray<T>(T[] src, int[] dim)
        {
            // Если измерений много
            if (dim.Length >= 2)
            {
                // Посчитаем объем ячеек первого измерения
                int mul = src.Length / dim[0];

                // Выделим место под данные об измерениях
                int[] dim_t = new int[dim.Length - 1];

                // Скопируем данные об измерениях кроме первого
                Array.Copy(dim, 1, dim_t, 0, dim_t.Length);

                // Дробим массив
                T[][] result = SplitArray<T>(src, dim[0], mul);

                // Вызовем разделение снова для каждой ячейки
                for (int i = 0; i < result.Length; i++)
                {
                    // 
                    result[i] = SplitArray<T>(result[i], dim_t) as T[];
                }

                // 
                return result;
            }
            else
            {
                // Вернем как есть
                return src;
            }
        }  
        public static A SplitArray<T, A>(T[] src, A dst)
        {
            // Получим размерность массива
            int[] dim = new int[(dst as Array).Rank];

            // Переберем измерения
            for (int i = 0; i < dim.Length; i++)
            {
                // Получим границы измерений массива
                dim[i] = (dst as Array).GetLength(i);
            }

            // 
            int[] vec = new int[dim.Length];

            // Переберем все ячейки
            for (int i = 0; i < src.Length; i++)
            {
                // Установим значение
                (dst as Array).SetValue(src[i], vec);

                // Инкрементируем младший разряд
                ++vec[dim.Length - 1];

                // Переберем измерения вектора
                for (int j = dim.Length - 1; j >= 1; j--)
                {
                    // Если мы достигли предела
                    if (vec[j] == dim[j])
                    {
                        // Инкрементируем старший разряд
                        vec[j - 1]++;

                        // Обнулим координату
                        vec[j] = 0;
                    }
                }
            }

            // Ответ
            return dst;
        }

        public static string ConvertToString(sbyte[] arr)
        {
            char[] chr = Array.ConvertAll<sbyte, char>(arr, new Converter<sbyte, char>(Convert.ToChar));

            // Remove \0
            if (chr.Length > 0 && chr[chr.Length - 1] == '\0')
            {
                chr = chr.Take(chr.Length - 1).ToArray();
            }

            return new string(chr);
        }

        public static sbyte[] ConvertToSByte(string str)
        {
            // Add \0
            char[] chr = new char[str.Length + 1];

            Array.Copy(str.ToCharArray(), chr, str.Length);

            return Array.ConvertAll<char, sbyte>(chr, new Converter<char, sbyte>(Convert.ToSByte));
        }

        // 
        // Variable
        // 
        public void PushToPackBool(bool value)
        {
            toSend.AddRange(BitConverter.GetBytes(value));
        }
        public void PushToPackSByte(sbyte value)
        {
            toSend.Add((byte)value);
        }
        public void PushToPackByte(byte value)
        {
            toSend.Add(value);
        }
        
        public void PushToPackInt(short value)
        {
            toSend.AddRange(BitConverter.GetBytes(value));
        }
        public void PushToPackInt(int value)
        {
            toSend.AddRange(BitConverter.GetBytes((short)value));
        }
        public void PushToPackUInt(ushort value)
        {
            toSend.AddRange(BitConverter.GetBytes(value));
        }
        public void PushToPackUInt(uint value)
        {
            toSend.AddRange(BitConverter.GetBytes((ushort)value));
        }

        public void PushToPackLong(int value)
        {
            toSend.AddRange(BitConverter.GetBytes(value));
        }
        public void PushToPackULong(uint value)
        {
            toSend.AddRange(BitConverter.GetBytes(value));
        }
        public void PushToPackFloat(float value)
        {
            toSend.AddRange(BitConverter.GetBytes(value));
        }
        public void PushToPackDouble(double value)
        {
            toSend.AddRange(BitConverter.GetBytes((float)value));
        }


        // 
        // Arrays
        // 
        public void PushToPackBool(bool[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackBool(value[i]);
        }
        
        public void PushToPackSByte(sbyte[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackSByte(value[i]);
        }
        public void PushToPackChar(char[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackSByte((sbyte)value[i]);
        }
        public void PushToPackString(string str)
        {
            PushToPackSByte(ConvertToSByte(str));
        }

        public void PushToPackInt(short[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackInt(value[i]);
        }
        public void PushToPackInt(int[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackInt(value[i]);
        }
        public void PushToPackUInt(ushort[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackUInt(value[i]);
        }
        public void PushToPackUInt(uint[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackUInt(value[i]);
        }

        public void PushToPackLong(int[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackLong(value[i]);
        }
        public void PushToPackULong(uint[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackULong(value[i]);
        }
        public void PushToPackFloat(float[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackFloat(value[i]);
        }
        public void PushToPackDouble(double[] value)
        {
            // Запишем длинну массива
            toSend.Add((byte)value.Length);

            // Затолкаем данные
            for (int i = 0; i < value.Length; i++) PushToPackDouble(value[i]);
        }



        public static T[] JoinArray<T>(object array)
        {
            // Получим размерность массива
            int[] dim = new int[(array as Array).Rank];

            // Переберем измерения и вычислим объем массива
            int len = 1;
            for (int i = 0; i < dim.Length; i++)
            {
                // Получим границы измерений массива
                dim[i] = (array as Array).GetLength(i);

                // Длина
                len *= dim[i];
            }

            // Выделим память под результат
            T[] res = new T[len];

            // Выделим память под координаты
            int[] vec = new int[dim.Length];

            // Переберем вектора
            for (int i = 0; i < len; i++)
            {
                // Поместим данные в результирующий массив
                res[i] = (T)(array as Array).GetValue(vec);

                // Инкрементируем младший разряд
                ++vec[dim.Length - 1];

                // Переберем измерения вектора
                for (int j = dim.Length-1; j >= 1; j--)
                {
                    // Если мы достигли предела
                    if (vec[j] == dim[j])
                    {
                        // Инкрементируем старший разряд
                        vec[j - 1]++;

                        // Обнулим координату
                        vec[j] = 0;
                    }
                }
            }

            // Ответ
            return res;
        }
    }

    // Events
    public delegate void OnConnectHandler();
    public event         OnConnectHandler OnConnect;
    
    public delegate void OnPackReceivedHandler(DATA data);
    public event         OnPackReceivedHandler OnPackReceived;

    // 
    // Variables
    // 
    public bool       ShowDebugLog     = true;

    bool              AllowConnect     = false;
    public bool       ConnectOnStart   = true;
    public bool       AutoReconnect    = true;
    public int        ReconnectAfterMs = 5000;
    SerialPort        serialPort       = new SerialPort();
    public string     PortName         = "COM4";
    public int        BaudRate         = 9600;
    

    // Стартовая последовательность
    byte[] startSequence = new byte[] { 255, 254, 253 };
    
    // Прием данных
    int        progress        = 0;
    int        progress2       = 0;
    byte[]     controlSum      = new byte[2];
    UInt16     controlSum2     = 0;
    int        receivePackLen  = 0;
    List<byte> receivePackData = new List<byte>();

    // Мониторинг
    public ulong BytesReceived   = 0;
    public ulong PacketsReceived = 0;

    // Connect / Disconnect
    public void Disconnect(bool disableAutoReConnect = false)
    {
        // Отключить реконнект
        if (disableAutoReConnect) AutoReconnect = false;

        // Если порт запущен
        if (serialPort.IsOpen)
        {
            // 
            if (ShowDebugLog) Debug.Log(serialPort.PortName + " " + serialPort.BaudRate + " Close");

            // Отключим порт
            serialPort.Close();
        }
    }
    public void Connect(bool enableAutoReConnect = false)
    {
        Connect2(enableAutoReConnect);
    }
    public bool Connect2(bool enableAutoReConnect = false)
    {
        // 
        AllowConnect = true;

        // 
        if (enableAutoReConnect) AutoReconnect = true;

        try
        {
            // 
            string[] ports = SerialPort.GetPortNames();

            // Выполним проверку имени
            if (!Array.Exists(ports, p => string.Compare(p, PortName, true) == 0)) return false;

            // Если изменилось имя
            if (string.Compare(serialPort.PortName, PortName, true) != 0)
            {
                // Закроем порт
                if (serialPort.IsOpen) serialPort.Close();

                // Установим имя
                serialPort.PortName = PortName;
            }

            // Если изменилась скорость
            if (serialPort.BaudRate != BaudRate)
            {
                // Закроем порт
                if (serialPort.IsOpen) serialPort.Close();

                // Установим скорость
                serialPort.BaudRate = BaudRate;
            }

            // Запустим порт
            if (!serialPort.IsOpen) serialPort.Open();

            // 
            if (ShowDebugLog) Debug.Log(serialPort.PortName + " " + serialPort.BaudRate + " Open");

        }
        catch (System.IO.IOException SerialException)
        {
            // Выведем сообщение об ошибке
            if (ShowDebugLog) Debug.Log(SerialException.Message);
        }

        // 
        if (OnConnect != null) OnConnect();

        // 
        return true;
    }
    
    IEnumerator ConnectTracking()
    {
        // Если соединение потеряно
        yield return new WaitUntil(() => AllowConnect && AutoReconnect && !serialPort.IsOpen);

        // Выждем
        StartCoroutine(ReconnectWaiting());
    }
    IEnumerator ReconnectWaiting()
    {
        // Выждем
        yield return new WaitForSecondsRealtime((float)ReconnectAfterMs / 1000.0f);
        
        // Попробуем подключиться
        Connect();

        // Продолжим слежение
        StartCoroutine(ConnectTracking());
    }

    // Прием данных
    IEnumerator ReceiveDataTracking()
    {
        yield return new WaitUntil(() => TrySerialPortBytesToRead());
        
        try
        {
            // Получим количество байт
            int rawLength = serialPort.BytesToRead;

            // 
            BytesReceived += (ulong)rawLength;

            // Считаем все данные из порта
            byte[] raw = new byte[rawLength];
            serialPort.Read(raw, 0, rawLength);
            
            // Переберем данные побайтово
            for (int i = 0; i < rawLength; i++)
            {
                // Анализ
                ReceiveDataProcessing(raw[i]);
            }

            // Продолжим чтение
            StartCoroutine(ReceiveDataTracking());
        }
        catch (System.IO.IOException SerialException)
        {
            // Выведем сообщение об ошибке
            if (ShowDebugLog) Debug.Log(SerialException.Message);

            // Сбросим прогресс
            ReceiveProgressReset();

            // Закроем порт
            Disconnect();
        }
        catch (InvalidOperationException SerialException)
        {
            if (ShowDebugLog) Debug.Log(SerialException.ToString());

            // Сбросим прогресс
            ReceiveProgressReset();

            // Закроем порт
            Disconnect();
        }
        catch
        {
            if (ShowDebugLog) Debug.Log("ERROR in Opening Serial Port -- UnKnown ERROR");

            // Сбросим прогресс
            ReceiveProgressReset();

            // Закроем порт
            Disconnect();
        }
    }
    void ReceiveProgressReset()
    {
        // Сбросим прогрессы
        progress       = 0;
        progress2      = 0;
        controlSum     = new byte[2];
        controlSum2    = 0;
        receivePackLen = 0;
        receivePackData.Clear();
    }
    void ReceiveDataProcessing(byte b)
    {
        // Ищем стартовую последовательность
        if (progress < startSequence.Length)
        {
            // Если байт подошел
            if (b == startSequence[progress])
            {
                // Нарастим прогресс
                progress++;
            }
            // Если байт не подошел
            else
            {
                // Сбросим прогресс
                progress = 0;
            }
        }
        else
        {
            // Считываем два байта суммы данных
            if (progress < (startSequence.Length + 2))
            {
                // Считаем
                controlSum[progress - startSequence.Length] = b;

                // Нарастим прогресс
                progress++;
            }
            else
            {
                // Считываем байт количества данных данных
                if (progress < (startSequence.Length + 3))
                {
                    // Запомним
                    receivePackLen = b;

                    // Нарастим прогресс
                    progress++;
                }
                else
                {
                    // Считываем данные
                    if (progress2 < receivePackLen)
                    {
                        // Посчитаем сумму
                        controlSum2 += b;

                        // Добавим байт в список
                        receivePackData.Add(b);

                        // Нарастим прогресс
                        if (++progress2 == receivePackLen)
                        {
                            // Сравним контрольную сумму
                            if (controlSum2 == BitConverter.ToUInt16(controlSum, 0))
                            {
                                // Подготовим структуру
                                DATA d = new DATA();
                                d.raw = receivePackData.ToArray();

                                // Сбросим прогрессы
                                ReceiveProgressReset();

                                // Нарастим счетчик
                                PacketsReceived++;

                                // Отдадим данные на обработку
                                if (OnPackReceived != null) OnPackReceived(d);
                            }
                            else
                            {
                                // Сбросим прогрессы
                                ReceiveProgressReset();
                            }
                        }
                    }
                }
            }
        }
    }

    // Работа с буфером
    bool TrySerialPortBytesToRead()
    {
        try
        {
            return serialPort.IsOpen && serialPort.BytesToRead > 0;
        }
        catch (System.IO.IOException SerialException)
        {
            // Выведем сообщение об ошибке
            if (ShowDebugLog) Debug.Log(SerialException.Message);

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return false;
        }
        catch (InvalidOperationException SerialException)
        {
            if (ShowDebugLog) Debug.Log(SerialException.ToString());

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return false;
        }
        catch
        {
            if (ShowDebugLog) Debug.Log("ERROR in Opening Serial Port -- UnKnown ERROR");

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return false;
        }
    }
    void TryDiscardInBuffer()
    {
        try
        {
            if (serialPort.IsOpen) serialPort.DiscardInBuffer();

            return;
        }
        catch (System.IO.IOException SerialException)
        {
            // Выведем сообщение об ошибке
            if (ShowDebugLog) Debug.Log(SerialException.Message);

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return;
        }
        catch (InvalidOperationException SerialException)
        {
            if (ShowDebugLog) Debug.Log(SerialException.ToString());

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return;
        }
        catch
        {
            if (ShowDebugLog) Debug.Log("ERROR in Opening Serial Port -- UnKnown ERROR");

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return;
        }
    }
    void TryDiscardOutBuffer()
    {
        try
        {
            if (serialPort.IsOpen) serialPort.DiscardOutBuffer();

            return;
        }
        catch (System.IO.IOException SerialException)
        {
            // Выведем сообщение об ошибке
            if (ShowDebugLog) Debug.Log(SerialException.Message);

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return;
        }
        catch (InvalidOperationException SerialException)
        {
            if (ShowDebugLog) Debug.Log(SerialException.ToString());

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return;
        }
        catch
        {
            if (ShowDebugLog) Debug.Log("ERROR in Opening Serial Port -- UnKnown ERROR");

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return;
        }
    }

    // Передать данные
    public void TransceiveData(DATA d)
    {
        try
        {
            if (serialPort.IsOpen)
            {
                // Посчитаем сумму байт пакета
                ushort summ = 0;
                for (int i = 0; i < d.toSend.Count; i++)
                {
                    summ += d.toSend[i];
                }

                serialPort.Write(startSequence,                               0, startSequence.Length);
                serialPort.Write(BitConverter.GetBytes(summ),                 0, 2);
                serialPort.Write(BitConverter.GetBytes((byte)d.toSend.Count), 0, 1);
                serialPort.Write(d.toSend.ToArray(),                          0, d.toSend.Count);
            }
        }
        catch (System.IO.IOException SerialException)
        {
            // Выведем сообщение об ошибке
            if (ShowDebugLog) Debug.Log(SerialException.Message);

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return;
        }
        catch (InvalidOperationException SerialException)
        {
            if (ShowDebugLog) Debug.Log(SerialException.ToString());

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return;
        }
        catch
        {
            if (ShowDebugLog) Debug.Log("ERROR in Opening Serial Port -- UnKnown ERROR");

            // Закроем порт
            Disconnect();

            // Вернем неудачу
            return;
        }
    }


    // Закрыть порт при выходе
    void OnDestroy()
    {
        // Сбросим прогресс приема
        ReceiveProgressReset();

        // Отконнектимся
        Disconnect();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Запустим слежение
        StartCoroutine(ReceiveDataTracking());

        // Запустим слежение
        StartCoroutine(ConnectTracking());

        // 
        if (ConnectOnStart) Connect();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
