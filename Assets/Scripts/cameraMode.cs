using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class cameraMode : MonoBehaviour
{
	private int camMode;
	public GameObject[] cam;
	private Transform drone;
	public Transform droneBody;
	
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
		Invoke("SetDrone", 0.5f);
	}
	
	private void SetDrone()
	{
		drone = GameObject.Find("Null Rotation Object").GetComponent<Transform>();
		droneBody = GameObject.FindGameObjectWithTag("Drone").GetComponent<Transform>();
		cam[2].SetActive(false);
		cam[2].AddComponent<AudioListener>();
	}
	
	public void EditCam()
	{
		camMode++;
		if (camMode > 3)
		{
			camMode = 0;
		}
		
		for (int i = 0; i < cam.Length; i++)
		{
			cam[i].SetActive(false);
		}
		
		if (camMode == 0) {
			cam[0].SetActive(true);
			cam[0].SetActive(true);
		}
		else if (camMode == 1) {
			cam[1].SetActive(true);
			cam[1].transform.position = new Vector3 (droneBody.position.x, droneBody.position.y - 0.25f, droneBody.position.z);
			cam[1].transform.rotation = Quaternion.Euler(droneBody.eulerAngles.x, droneBody.eulerAngles.y, droneBody.eulerAngles.z);
		}
		else if (camMode == 2) {
			cam[1].SetActive(true);
			float slider1 = inputControl.Drone.slider1.ReadValue<float>();
			cam[1].transform.position = new Vector3 (droneBody.position.x, droneBody.position.y - 0.25f, droneBody.position.z);
			cam[1].transform.rotation = Quaternion.Euler(droneBody.eulerAngles.x + 50 + slider1 * 30, droneBody.eulerAngles.y, droneBody.eulerAngles.z);
		}
		else if (camMode == 3) {
			cam[1].SetActive(true);
			cam[1].transform.position = new Vector3 (droneBody.position.x, droneBody.position.y - 0.25f, droneBody.position.z);
			cam[1].transform.rotation = Quaternion.Euler(droneBody.eulerAngles.x, droneBody.eulerAngles.y, droneBody.eulerAngles.z);
		}
	}

	public void Cam3OnOff()
	{
		if (cam[2].activeInHierarchy)
		{
			cam[2].SetActive(false);
			camMode--;
			EditCam();
		}
		else
		{
			for (int i = 0; i < cam.Length; i++)
			{
				cam[i].SetActive(false);
			}
			cam[2].SetActive(true);
		}
	}

	void Update()
	{
		if (camMode == 2)
		{
			float slider1 = inputControl.Drone.slider1.ReadValue<float>();
			cam[1].transform.position = new Vector3 (droneBody.position.x, droneBody.position.y - 0.25f, droneBody.position.z);
			cam[1].transform.rotation = Quaternion.Euler(droneBody.eulerAngles.x + 50 + slider1 * 30, droneBody.eulerAngles.y, 0);
		}
		else if (camMode == 1 || camMode == 3)
		{
			cam[1].transform.position = new Vector3 (droneBody.position.x, droneBody.position.y - 0.25f, droneBody.position.z);
			cam[1].transform.rotation = Quaternion.Euler(droneBody.eulerAngles.x, droneBody.eulerAngles.y, 0);
		}

		if (droneBody.GetComponent<PathFollower>())
		{
			cam[1].transform.rotation = Quaternion.Euler(0, droneBody.eulerAngles.y + 90, 0);
		}
	}
}
