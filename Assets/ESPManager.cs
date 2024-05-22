using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Communicate_With_ESP32_via_Bluetooth_Serial;
public class ESPManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MainClass.Main();
    }

    // Update is called once per frame
    void Update()
    {
        //MainClass.Read();
    }
}
