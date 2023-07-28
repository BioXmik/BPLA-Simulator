using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BasicControl : MonoBehaviour {

    private GameObject nullRotationObject;

	[Header("Control")]
	public Controller Controller;
	public float ThrottleIncrease;
	
	[Header("Motors")]
	public Motor[] Motors;
	public float ThrottleValue;

    [Header("Internal")]
    public ComputerModule Computer;

    private Sensors sensors;

    void Awake()
    {
        sensors = GetComponent<Sensors>();
        nullRotationObject = new GameObject("Null Rotation Object");
        nullRotationObject.tag = "Player";
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DroneCamera>().ourDrone = nullRotationObject;
    }

	void FixedUpdate() {
        nullRotationObject.transform.position = transform.position;
        nullRotationObject.transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
        Computer.UpdateComputer(Controller.Pitch, Controller.Roll, Controller.Throttle * ThrottleIncrease);
        ThrottleValue = Computer.HeightCorrection;
        ComputeMotors();
        if (Computer != null)
            Computer.UpdateGyro();
        ComputeMotorSpeeds();
	}

    private void ComputeMotors()
    {
        if (!sensors.killSwitch)
        {
        float yaw = 0.0f;
        Rigidbody rb = GetComponent<Rigidbody>();
        int i = 0;
        foreach (Motor motor in Motors)
        {
            motor.UpdateForceValues();
            yaw += motor.SideForce;
            i++;
            Transform t = motor.GetComponent<Transform>();
            rb.AddForceAtPosition(transform.up * motor.UpForce, t.position, ForceMode.Impulse);
        }
        rb.AddTorque(Vector3.up * yaw, ForceMode.Force);
        }
    }

    private void ComputeMotorSpeeds()
    {
        if (!sensors.killSwitch)
        {
        foreach (Motor motor in Motors)
        {
            if (Computer.Gyro.Altitude < 0.1)
                motor.UpdatePropeller(0.0f);
            else
                motor.UpdatePropeller(1200.0f);
        }
        }
    }
}