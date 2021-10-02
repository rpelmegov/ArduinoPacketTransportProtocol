using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_02 : MonoBehaviour
{
    // Class object reference
    public PTP port;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SendData()
    {
        // 
        // Transfer data
        // 
        PTP.DATA d = new PTP.DATA();

        // 
        // Basic primitive types
        // 
        d.PushToPackBool(true);
        d.PushToPackSByte(-128);
        d.PushToPackByte(255);
        d.PushToPackInt(1000);
        d.PushToPackUInt(2000);
        d.PushToPackLong(3000);
        d.PushToPackULong(4000);
        d.PushToPackFloat(5000);
        d.PushToPackDouble(6000);

        // 
        // Arrays
        // 
        d.PushToPackInt(new int[] { 0, 1, 2 });

        // Transfering a multidimensional array
        int[,,] mdarr_v1 = new int[2, 1, 2];
        mdarr_v1[0,0,0] = 1;
        mdarr_v1[0,0,1] = 2;
        mdarr_v1[1,0,0] = 3;
        mdarr_v1[1,0,1] = 4;

        int[] odarr_v1 = PTP.DATA.JoinArray<int>(mdarr_v1);  // Step 1 - Convert to one dimension array
        d.PushToPackInt(odarr_v1);                           // Step 2 - Add to pack

        // 
        // 
        // 

        // 
        // String
        // 
        d.PushToPackString("Hello, world!");

        // 
        // Transfer
        // 
        port.TransceiveData(d);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
