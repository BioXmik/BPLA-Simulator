using UnityEngine;
using System;
using System.IO.Ports;

public class droneController : MonoBehaviour {

    private InputControl inputControl;

    private void Awake()
    {
        inputControl = new InputControl();
        inputControl.Drone.KillSwitch.performed += context => KillSwitch();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    void Update()
    {
        Vector3 direction = inputControl.Drone.Move.ReadValue<Vector3>();

        Move(direction);
    }

    private void Move(Vector3 newVector)
    {
        transform.position += newVector;
        //Debug.Log(newVector);
    }

    void KillSwitch()
    {
        Debug.Log("KillSwitch");
    }
}