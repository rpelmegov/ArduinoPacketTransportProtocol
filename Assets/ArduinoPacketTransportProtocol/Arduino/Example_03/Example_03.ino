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
  // Transfer data every 1 second
  // 
  // 
  if (millis() - last_time >= 1000)
  {
    last_time = millis();
    
    char commandPing[] = "Ping";
    byte commandLen    = sizeof(commandPing);
    
    port->BeginPack();
    port->PushToPack(&commandLen);
    port->PushToPack(&commandPing);
    port->SendPack();
  }

  // 
  // Listen port
  // 
  if (port->Listen())
  {
    char commandTime[] = "Time";
    byte commandLen    = sizeof(commandTime);
    
    // 
    float server_time = port->GetVal<float>();
    
    // 
    port->BeginPack();
    port->PushToPack(&commandLen);
    port->PushToPack(&commandTime);
    port->PushToPack(&server_time);
    port->SendPack();
    
    // Blink
    digitalWrite(13, HIGH);
    delay(50);
    digitalWrite(13, LOW);
    delay(50);
  }
}

