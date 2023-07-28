using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMode : MonoBehaviour
{
	private int camMode;
	public GameObject[] cam;
	private Transform drone;
	
	private InputControl inputControl;

    private void Awake()
    {
        inputControl = new InputControl();
        inputControl.Drone.EditCam.performed += context => EditCam();
		inputControl.Drone.EditCam.canceled += context => EditCam();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

	void Start()
	{
		Invoke("SetDrone", 0.1f);
	}
	
	private void SetDrone()
	{
		drone = GameObject.Find("Null Rotation Object").GetComponent<Transform>();
	}
	
	public void EditCam()
	{
		cam[camMode].SetActive(false);
		camMode++;
		if (camMode > 2) {camMode = 0;}
		cam[camMode].SetActive(true);
		
		if (camMode == 1)
		{
			float slider1 = inputControl.Drone.slider1.ReadValue<float>();
			cam[1].transform.position = new Vector3 (drone.position.x, drone.position.y - 0.25f, drone.position.z);
			cam[1].transform.rotation = Quaternion.Euler(drone.eulerAngles.x + 50 + slider1 * 30, drone.eulerAngles.y, drone.eulerAngles.z);
		}
	}

	void Update()
	{
		if (camMode == 1)
		{
			float slider1 = inputControl.Drone.slider1.ReadValue<float>();
			cam[1].transform.position = new Vector3 (drone.position.x, drone.position.y - 0.25f, drone.position.z);
			cam[1].transform.rotation = Quaternion.Euler(drone.eulerAngles.x + 50 + slider1 * 30, drone.eulerAngles.y, drone.eulerAngles.z);
		}
	}
}
