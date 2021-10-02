using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_04 : MonoBehaviour
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

    public void SendData_1()
    {
        // 
        // Transfer command
        // 
        PTP.DATA d = new PTP.DATA();
        d.PushToPackByte(1);
        port.TransceiveData(d);
    }
    public void SendData_2()
    {
        // 
        // Transfer command
        // 
        PTP.DATA d = new PTP.DATA();
        d.PushToPackByte(2);
        port.TransceiveData(d);
    }

    // 
    // Receive data event
    // 
    void PackReceiveProcessing(PTP.DATA data)
    {
        uint arduino_time = data.GetULong();

        Debug.Log("Server launched already " + (int)((float)arduino_time/1000) + " seconds");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
