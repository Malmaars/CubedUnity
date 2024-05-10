using UnityEngine;
using System;
using System.IO.Ports;

namespace Communicate_With_ESP32_via_Bluetooth_Serial
{
    class MainClass
    {
        public static void Main()
        {
            Debug.Log("Starting ESP connnection");

            try
            {
                SerialPort port = new SerialPort();
                port.BaudRate = 9600;
                port.PortName = "COM9";
                port.Open();

                try
                {
                    port.Write("Hi, I am trying to talk to you.");
                    Debug.Log(port.ReadLine());
                    Debug.Log(port.ReadLine());

                    port.Write("Why do you have to repeat what I say?");
                    Debug.Log(port.ReadLine());
                    Debug.Log(port.ReadLine());
                }
                catch (Exception ex)
                {
                    Debug.Log("Encountered error while writing to / or reading from serial port");
                    Debug.Log(ex.ToString());
                }

            }
            catch (Exception ex)
            {
                Debug.Log("Encountered error while opening serial port");
                Debug.Log(ex.ToString());
            }
        }
    }
}