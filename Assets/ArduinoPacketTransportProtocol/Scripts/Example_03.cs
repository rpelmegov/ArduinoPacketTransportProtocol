using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_03 : MonoBehaviour
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
        // Get the command
        // 
        int     charArrayLen = data.GetByte();                          // Step 1
        sbyte[] charArray    = data.GetChar(charArrayLen);              // Step 2
        string  command      = PTP.DATA.ConvertToString(charArray);  // Step 3

        // 
        switch(command)
        {
            case "Ping":

                // Print command
                Debug.Log("Ping");

                // Send a response
                PTP.DATA d = new PTP.DATA();
                d.PushToPackFloat(Time.realtimeSinceStartup);
                port.TransceiveData(d);

                break;

            case "Time":

                // Get Time
                float res_time = data.GetFloat();

                // Print command
                Debug.Log("Two way time " + (int)((Time.realtimeSinceStartup - res_time) * 1000) + " ms");

                break;

            default:
                Debug.Log("Unknown command");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
