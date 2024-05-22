using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Uduino;

public class ReadEsp32 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UduinoManager.Instance.Read();   
    }
}
