using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_00 : MonoBehaviour
{
    public PTP port;

    public uint time;

    void Start()
    {
        // On Receive data
        port.OnPackReceived -= PackReceiveProcessing;
        port.OnPackReceived += PackReceiveProcessing;
    }

    void PackReceiveProcessing(PTP.DATA data)
    {
        time = data.GetULong();
    }

    void Update()
    {
        
    }
}
