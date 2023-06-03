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
	public Transform rotationDron;
	
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
	
	void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		StartCoroutine(AddTime());
		oldPos = transform.position;
		home = transform.position;
		killSwitchText.text = "Kill Switch: " + killSwitch.ToString();
		
		if (PlayerPrefs.GetInt("Mode") == 0) {mode = "stab";}
		if (PlayerPrefs.GetInt("Mode") == 1) {mode = "althold";}
		if (PlayerPrefs.GetInt("Mode") == 2) {mode = "loiter";}
		if (PlayerPrefs.GetInt("Mode") == 3) {mode = "auto";}
	}
	
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton1))
		{
			SceneManager.LoadScene("Menu");
		}
			
		if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.JoystickButton2))
		{
			killSwitchOnOff();
		}
			
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
		
		if (lineGorizont != null && rotationDron != null)
		{
			lineGorizont.rotation = Quaternion.Euler(0, 0, rotationDron.localEulerAngles.z * 4);
			lineGorizont.transform.position = new Vector2 (Screen.width / 2 + rotationDron.rotation.z * -1000, Screen.height / 2 + rotationDron.rotation.x * 1000);
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
			height.text = Math.Round(transform.position.y, 3).ToString() + " m";
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
	
	public void killSwitchOnOff()
	{
		if (killSwitch)
		{
			killSwitch = false;
			GetComponent<battery>().consumption = 21.2f;
		}
		else
		{
			killSwitch = true;
			GetComponent<battery>().consumption = 0f;
		}
		GetComponent<battery>().consumptionText.text = GetComponent<battery>().consumption.ToString();
		killSwitchText.text = "Kill Switch: " + killSwitch.ToString();
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
