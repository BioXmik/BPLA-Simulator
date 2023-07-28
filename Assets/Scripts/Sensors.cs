using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sensors : MonoBehaviour
{
	public Text speedVertical;
	public Text speedHorizontal;
	
	public Text height;
	
	private Rigidbody rigidbody;
	
	public RectTransform lineGorizont;
	
	private int timeS;
	private int timeMin;
	public Text Time;
	
	private double allDistance;
	public Text allDistanceText;
	private Vector3 oldPos;
	
	public Text homeDistance;
	private Vector3 home;
	
	public bool killSwitch;
	public Text killSwitchText;
	
	public string mode;
	public Text modeText;
	
	public float earthDistance;
	private Terrain terrain;
	
	private InputControl inputControl;

    private void Awake()
    {
        inputControl = new InputControl();
        inputControl.Drone.KillSwitch.performed += context => killSwitchOnOff(false);
		inputControl.Drone.KillSwitch.canceled += context => killSwitchOnOff(true);
		inputControl.Drone.Menu.performed += context => OnMenu();
		inputControl.Drone.Menu.canceled += context => OnMenu();
		inputControl.Drone.EditMode.performed += context => EditMode();
		inputControl.Drone.EditMode.canceled += context => EditMode();
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
		rigidbody = GetComponent<Rigidbody>();
		StartCoroutine(AddTime());
		oldPos = transform.position;
		home = transform.position;
		killSwitchText.text = "Kill Switch: " + killSwitch.ToString();
		terrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<Terrain>();
		
		if (PlayerPrefs.GetInt("Mode") == 0) {mode = "stab";}
		else if (PlayerPrefs.GetInt("Mode") == 1) {mode = "althold";}
		else if (PlayerPrefs.GetInt("Mode") == 2) {mode = "loiter";}
		else if (PlayerPrefs.GetInt("Mode") == 3) {mode = "auto";}
	}
	
    void Update()
    {
		earthDistance = transform.position.y - terrain.SampleHeight(transform.position);
		
		if (modeText != null)
		{
			modeText.text = "Mode: " + mode.ToString();
		}
		
		if (allDistanceText != null)
		{
			allDistance = allDistance + Math.Round(Vector3.Distance(transform.position, oldPos), 2);
			allDistanceText.text = allDistance + "m";
			oldPos = transform.position;
		}
		
		if (homeDistance != null)
		{
			homeDistance.text = Math.Round(Vector3.Distance(transform.position, home), 2).ToString() + "m";
		}
		
		if (lineGorizont != null)
		{
			float rotationZ = transform.eulerAngles.z;
			if (rotationZ > 180) {rotationZ -= 360;}

			float rotationX = transform.eulerAngles.x;
			if (rotationX > 180) {rotationX -= 360;}
    		
			lineGorizont.rotation = Quaternion.Euler(0, 0, rotationZ);
			lineGorizont.transform.position = new Vector2 (Screen.width / 2 + rotationZ * -1, Screen.height / 2 + rotationX * 1);
		}
		
		if (speedVertical != null)
		{
			double speed = Math.Round(rigidbody.velocity.y, 2);
			
			if (speed < 0)
			{
				speed =  speed * -1;
			}
			
			speedVertical.text = speed.ToString() + " km/h";
		}
		
		if (speedHorizontal != null)
		{
			double speed;
			double speedX = rigidbody.velocity.x;
			double speedZ = rigidbody.velocity.z;
			
			if (speedX < 0)
			{
				speedX =  speedX * -1;
			}
			
			if (speedZ < 0)
			{
				speedZ =  speedZ * -1;
			}
			
			speed = Math.Round(speedX + speedZ, 2);
			
			speedHorizontal.text = speed.ToString() + " km/h";
		}
		
		if (height != null)
		{
			height.text = Math.Round(earthDistance, 2).ToString() + " m";
		}
    }
	
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag != "NoTriggerDron" && collision.gameObject.tag != "Drop")
		{
			killSwitch = true;
			GetComponent<battery>().consumption = 0f;
		}
	}

	public void killSwitchOnOff(bool status)
	{
		killSwitch = status;
		if (killSwitch)
		{
			GetComponent<battery>().consumption = 21.2f;
		}
		else
		{
			GetComponent<battery>().consumption = 0f;
		}
		GetComponent<battery>().consumptionText.text = GetComponent<battery>().consumption.ToString();
		killSwitchText.text = "Kill Switch: " + killSwitch.ToString();
	}

	public void EditMode()
	{
		if (mode == "stab") {mode = "althold";}
		else if (mode == "althold") {mode = "loiter";}
		else if (mode == "loiter") {mode = "auto";}
		else if (mode == "auto") {mode = "stab";}
	}

	public void OnMenu()
	{
		SceneManager.LoadScene("Menu");
	}
	
	public IEnumerator AddTime()
	{
		yield return new WaitForSeconds (1f);
		timeS++;
		if (Time != null)
		{
			if (timeS >= 60)
			{
				timeS = 0;
				timeMin++;
			}
			
			Time.text = timeMin.ToString() + ":";
			
			if (timeS <= 9)
			{
				Time.text += "0" + timeS.ToString() + "m";
			}
			else
			{
				Time.text += timeS.ToString() + "m";
			}
		}
		StartCoroutine(AddTime());
	}
}
