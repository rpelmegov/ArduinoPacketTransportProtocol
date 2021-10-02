#include "TP4/STP4.h"

// Open serial port
STP4* port;

void setup()
{
  port = new STP4(115200);
}

void loop()
{
  // Prepare data
  unsigned long times = millis();
  
  // Ppackaging
  port->BeginPack();
  port->PushToPack(&times);

  // Transfer
  port->SendPack();
}

