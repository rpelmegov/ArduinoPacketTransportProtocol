using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_01 : MonoBehaviour
{
    // Class object reference
    public PTP port;

    // Start is called before the first frame update
    void Start()
    {
        // 
        // On Receive data
        // 
        port.OnPackReceived -= PackReceiveProcessing;
        port.OnPackReceived += PackReceiveProcessing;
    }

    // 
    // Receive data event
    // 
    void PackReceiveProcessing(PTP.DATA data)
    {
        // 
        // Basic primitive types
        // 
        bool   val_bool   = data.GetBool();
        int    val_char   = data.GetChar();
        byte   val_byte   = data.GetByte();
        int    val_int    = data.GetInt();
        int    val_uint   = data.GetUInt();
        int    val_long   = data.GetLong();
        uint   val_ulong  = data.GetULong();
        float  val_float  = data.GetFloat();
        double val_double = data.GetDouble();

        // 
        // Arrays
        // 
        int[] staticArray = data.GetInt(3);

        float[] dynamicArray = data.GetFloat(3);

        int[]   multidimensionalArray = data.GetInt(15);                                                // Step 1
        int[][] mdarr_v1 = PTP.DATA.SplitArray (multidimensionalArray, 3, 5);                           // Step 2
        int[][] mdarr_v2 = PTP.DATA.SplitArray (multidimensionalArray, new int[] { 3, 5 }) as int[][];  // Other variant Step 2 works with arrays of any dimension
        int[,]  mdarr_v3 = new int[3, 5]; PTP.DATA.SplitArray(multidimensionalArray, mdarr_v3);         // Other variant Step 2 works with Rectangular array of any dimension

        // 
        // String
        // 
        int     charArrayLen    = data.GetByte();                       // Step 1
        sbyte[] charArray       = data.GetChar(charArrayLen);           // Step 2
        string  charArrayString = PTP.DATA.ConvertToString(charArray);  // Step 3

        // 
        // Visualization of received data
        // 
        print("-= Pack Data = -");
        // print(val_bool);
        // print(val_char);
        // print(val_byte);
        print(val_int);
        // print(val_uint);
        // print(val_long);
        // print(val_ulong);
        // print(val_float);
        // print(val_double);
        // 
        // print(string.Join(",", staticArray));
        // 
        // print(string.Join(",", dynamicArray));
        // 
        // print(string.Join(",", multidimensionalArray));
        // print(string.Join(",", mdarr_v1[0]));
        // print(string.Join(",", mdarr_v1[1]));
        // print(string.Join(",", mdarr_v1[2]));
        // print(string.Join(",", mdarr_v2[0]));
        // print(string.Join(",", mdarr_v2[1]));
        // print(string.Join(",", mdarr_v2[2]));

        // print(charArrayLen + " char elements: " + string.Join(",", charArray));
        print(charArrayString);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
