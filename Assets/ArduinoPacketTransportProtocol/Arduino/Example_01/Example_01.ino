#include "TP4/STP4.h"

// Port speed
#define baudRate 115200

// Open serial port
STP4* port;

void setup()
{
  port = new STP4(baudRate);
  
  pinMode(13, OUTPUT);
  digitalWrite(13, LOW);
}

void loop()
{
  // 
  // Transfer data
  // 
  // 
  // 0. Prepare data
  // 
  bool          val_bool   = true;
  char          val_char   = -128;
  byte          val_byte   = 255;
  int           val_int    = 1000;
  unsigned int  val_uint   = 2000;
  long          val_long   = 3000;
  unsigned long val_ulong  = 4000;
  float         val_float  = 5000;
  double        val_double = 6000;

  int staticArray[3] = { 0, 1, 2 };

  int multidimensionalArray[3][5] = {
    { 1, 2, 3, 4, 5 },
    { 6, 7, 8, 9, 10 },
    { 11, 12, 13, 14, 15 } };

  float* dynamicArray = new float[3];// { 3, 4, 5 };
  dynamicArray[0] = 3;
  dynamicArray[1] = 4;
  dynamicArray[2] = 5;
  
  char charArray[]  = "Hello, world!";
  byte charArrayLen = sizeof(charArray);
  
  // 
  // 1.Start of packaging
  // 
  port->BeginPack();
  
  // 
  // 2. Adding data
  // 
  port->PushToPack(&val_bool);
  port->PushToPack(&val_char);
  port->PushToPack(&val_byte);
  port->PushToPack(&val_int);
  port->PushToPack(&val_uint);
  port->PushToPack(&val_long);
  port->PushToPack(&val_ulong);
  port->PushToPack(&val_float);
  port->PushToPack(&val_double);
  
  port->PushToPack(&staticArray);
  
  // Attention! Transmitting a dynamic array
  port->PushToPack(dynamicArray, 3);
  
  port->PushToPack(&multidimensionalArray);
  
  port->PushToPack(&charArrayLen);
  port->PushToPack(&charArray);
  
  
  // 
  // 3. Transfer command
  // 
  port->SendPack();
  
  // 4. Clear RAM
  delete[] dynamicArray;

  // Blink
  digitalWrite(13, HIGH);
  delay(50);
  digitalWrite(13, LOW);
  delay(50);
}

