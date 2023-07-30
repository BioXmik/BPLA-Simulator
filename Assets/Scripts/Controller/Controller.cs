using System;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	public float Throttle = 0.0f;
	public float Yaw = 0.0f;
	public float Pitch = 0.0f;
	public float Roll = 0.0f;

    public enum ThrottleMode { None, LockHeight};

	[Header("Throttle command")]
	public string ThrottleCommand = "Throttle";
	public bool InvertThrottle = true;

    [Header("Yaw Command")]
	public string YawCommand = "Yaw";
	public bool InvertYaw = false;

	[Header("Pitch Command")]
	public string PitchCommand = "Pitch";
	public bool InvertPitch = true;

	[Header("Roll Command")]
	public string RollCommand = "Roll";
	public bool InvertRoll = true;

	private AudioSource audioSource;
	private Sensors sensors;
	private InputControl inputControl;

    private void Awake()
    {
		audioSource = GetComponent<AudioSource>();
		sensors = GetComponent<Sensors>();
        inputControl = new InputControl();
        //inputControl.Drone.KillSwitch.performed += context => killSwitchOnOff();
		//inputControl.Drone.Menu.performed += context => OnMenu();
		//inputControl.Drone.EditMode.performed += context => EditMode();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

	void Update() {
        //Throttle = Input.GetAxisRaw(ThrottleCommand) * (InvertThrottle ? -1 : 1);
        //Yaw = Input.GetAxisRaw(YawCommand) * (InvertYaw ? -1 : 1);
        //Pitch = Input.GetAxisRaw(PitchCommand) * (InvertPitch ? -1 : 1);
        //Roll = Input.GetAxisRaw(RollCommand) * (InvertRoll ? -1 : 1);
		
		if (!sensors.killSwitch)
        {
			Vector3 direction = inputControl.Drone.Move.ReadValue<Vector3>();
			float forwardBackward = inputControl.Drone.ForwardBackward.ReadValue<float>();
			float leftRight = inputControl.Drone.LeftRight.ReadValue<float>();
			float slider2 = inputControl.Drone.slider2.ReadValue<float>();

			Throttle = direction.y * (InvertThrottle ? -1 : 1);
			Yaw = direction.x * (InvertYaw ? -1 : 1);
			Pitch = slider2 / 6 + forwardBackward * (InvertPitch ? -1 : 1);
			Roll = leftRight * (InvertRoll ? -1 : 1);

			float[] powers = new float[] {Math.Abs(direction.x), Math.Abs(direction.y), Math.Abs(leftRight), Math.Abs(forwardBackward)};
			audioSource.pitch = 0.8f + powers.Max();
		}
	}

}
