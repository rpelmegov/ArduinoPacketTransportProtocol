#ifndef __STP4_h__
#define __STP4_h__

#include "TP4.h"

// 
class STP4 : public TP4
{
public:
	int  Available() { Serial.available(); }
	int  Read()      { Serial.read(); }
	int  Peek()      { Serial.peek(); }
	void Flush()     { Serial.flush(); }

	STP4(long baudRate = 9600)
	{
    // Установим стартовую последовательность
    SetStartSequence( new byte[3] { 255, 254, 253 }, 3 );
    
		// Запустим порт
		Serial.begin(baudRate);
	}

	void SendBytes(byte* arr, byte len)
	{
		Serial.write(arr, len);
	}
};

#endif

