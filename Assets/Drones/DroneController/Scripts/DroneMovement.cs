﻿using DroneController.Physics;

public class DroneMovement : DroneMovementScript {

	private battery batteryScript;
	private Sensors sensorsScript;

	void Start()
	{
		batteryScript = GetComponent<battery>();
		sensorsScript = GetComponent<Sensors>();
	}

    public override void Awake()
    {
        base.Awake(); //I would suggest you to put code below this line or in a Start() method
    }

    void FixedUpdate()
    {
		if (batteryScript.energy > 0 && !sensorsScript.killSwitch)
		{
			GetVelocity();
			ClampingSpeedValues();
			SettingControllerToInputSettings(); //sensitivity settings for joystick,keyboard,mobile (depending on which is turned on)
			if (FlightRecorderOverride == false)
			{
				MovementUpDown();
				MovementLeftRight();
				Rotation();
				MovementForward();
				BasicDroneHoverAndRotation(); //this method applies all the forces and rotations to the drone.
			}
		}
	}

    void Update () {
		if (batteryScript.energy > 0 && !sensorsScript.killSwitch)
		{
			RotationUpdateLoop_TrickRotation(); //applies rotation to the drone it self when doing the barrel roll trick, does NOT trigger the animation
			Animations(); //part where animations are triggered
			DroneSound(); //sound producing stuff
			CameraCorrectPickAndTranslatingInputToWSAD(); //setting input for keys, translating joystick, mobile inputs as WSAD (depending on which is turned on)
		}
    }

}
