using DroneController.CameraMovement;

public class DroneCamera : CameraScript {

	private void FixedUpdate()
	{
		FPVTPSCamera();
		ScrollMath();
	}

	void Update()
    {
	
		PickDroneToControl();
    }

}
