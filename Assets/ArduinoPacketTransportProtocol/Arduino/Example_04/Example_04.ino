#include "TP4/STP4.h"

// Port speed
#define baudRate 9600

// Open serial port
STP4* port;

// 
unsigned long last_time = 0;

void setup()
{
  port = new STP4(baudRate);
  
  pinMode(13, OUTPUT);
  digitalWrite(13, LOW);
}

void loop()
{
  // 
  // Listen port
  // 
  if (port->Listen())
  {
    byte command = port->GetVal<byte>();

    switch(command)
    {
      case 1:
        // Blink
        digitalWrite(13, HIGH);
        delay(50);
        digitalWrite(13, LOW);
        delay(50);
        break;

      case 2:
        unsigned long t = millis();
        port->BeginPack();
        port->PushToPack(&t);
        port->SendPack();
        break;
    }
  }
}

