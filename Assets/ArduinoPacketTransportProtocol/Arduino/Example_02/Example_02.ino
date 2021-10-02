#include "TP4/STP4.h"

// Port speed
#define baudRate 9600

// Open serial port
STP4* port;

// 
char* str_test = "Hello, world!";

void setup()
{
  port = new STP4(baudRate);
  
  pinMode(13, OUTPUT);
  digitalWrite(13, LOW);
}

void loop()
{
  // 
  // Receiving data
  // 
  // 1. Wait Data
  if (port->Listen())
  {
    // 2. Get Data
    // 
    // Basic primitive types
    // 
    bool          val_bool   = port->GetVal<bool>();
    char          val_char   = port->GetVal<char>();
    byte          val_byte   = port->GetVal<byte>();
    int           val_int    = port->GetVal<int>();
    unsigned int  val_uint   = port->GetVal<unsigned int>();
    long          val_long   = port->GetVal<long>();
    unsigned long val_ulong  = port->GetVal<unsigned long>();
    float         val_float  = port->GetVal<float>();
    double        val_double = port->GetVal<double>();

    // 
    // Arrays
    // 
    int  arr_len;
    int* arr_int = port->GetArr<int>(&arr_len);
    //int* arr_int = port->GetArr<int>(); // You don't need to pass a pointer if you know the length of the array
    
    // Multidimensional array
    int  mda_int[2][1][2];        // Step 1 - You can set an array of any dimension
    port->GetArr<int>(&mda_int);  // Step 2
    
    // 
    // String
    // 
    char* str_chr = port->GetArr<char>();
    
    // 3. Any process
    // 
    // Visualization of received data
    // 
    if (
    val_bool   == true &&
    val_char   == -128 &&
    val_byte   == 255  &&
    val_int    == 1000 &&
    val_uint   == 2000 &&
    val_long   == 3000 &&
    val_ulong  == 4000 &&
    val_float  == 5000 &&
    val_double == 6000 &&
    arr_int[0] == 0 && arr_int[1] == 1 && arr_int[2] == 2 &&
    mda_int[0][0][0] == 1 && mda_int[0][0][1] == 2 && 
    mda_int[1][0][0] == 3 && mda_int[1][0][1] == 4 && 
    strcmp(str_chr, str_test) == 0 )
    {
      // Blink
      digitalWrite(13, HIGH);
      delay(50);
      digitalWrite(13, LOW);
      delay(50);
    }
    
    // 4. Clear RAM
    delete[] arr_int;
    delete[] str_chr;
  }
}

