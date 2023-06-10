using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMode : MonoBehaviour
{
	private int camMode;
	public GameObject[] cam;
	private Transform drone;
	
	void Start()
	{
		Invoke("SetDrone", 0.1f);
	}
	
	private void SetDrone()
	{
		drone = GameObject.FindGameObjectWithTag("AnimatedGO").GetComponent<Transform>();
	}
	
	public void Update()
	{
		//Смена камеры
		if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton5))
		{
			cam[camMode].SetActive(false);
			camMode++;
			if (camMode > 2) {camMode = 0;}
			cam[camMode].SetActive(true);
		}
		
		if (camMode == 1)
		{
			cam[1].transform.position = new Vector3 (drone.position.x, drone.position.y - 0.25f, drone.position.z);
			cam[1].transform.rotation = Quaternion.Euler(drone.eulerAngles.x + 40, drone.eulerAngles.y, drone.eulerAngles.z);
		}
	}
}
