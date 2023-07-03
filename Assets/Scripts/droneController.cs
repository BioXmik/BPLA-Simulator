using UnityEngine;
using System;
using System.IO.Ports;

public class droneController : MonoBehaviour {

    SerialPort serial;

    void Start () {
        serial = new SerialPort("COM3", 115200); // замените COM3 на имя порта, к которому подключен пульт
        serial.ReadTimeout = 1000; // время ожидания ответа в миллисекундах
        serial.Open();
    }

    void Update () {
        try {
            string data = serial.ReadLine();
            string[] values = data.Split(',');
            float roll = float.Parse(values[0]);
            float pitch = float.Parse(values[1]);
            float yaw = float.Parse(values[2]);
            float throttle = float.Parse(values[3]);

            Vector3 movement = new Vector3(roll * 50f, throttle * 50f, pitch * 50f);
            transform.Translate(movement * Time.deltaTime);

            transform.Rotate(new Vector3(0f, yaw * 50f, 0f) * Time.deltaTime);
        } catch (TimeoutException) {
            // обработка ошибки таймаута
        }
    }

    void OnApplicationQuit () {
        serial.Close();
    }
}