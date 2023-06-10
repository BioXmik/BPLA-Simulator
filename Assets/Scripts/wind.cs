using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
	private float speed;
	private float thisSpeed;
	private float windVector;
	
	public Rigidbody rbDrone;
	public GameObject drone;
	public Sensors sensors;
	
	public GameObject particle;
	
	void Start()
	{
		windVector = PlayerPrefs.GetFloat("WindVector");
		speed = PlayerPrefs.GetFloat("ValueWind") * -1;
		thisSpeed = PlayerPrefs.GetFloat("ValueWind") * -1;
		Debug.Log(PlayerPrefs.GetString("ChangingSpeedAndDirectionWind"));
		if (PlayerPrefs.GetString("ChangingSpeedAndDirectionWind") == "False")
		{
			StartCoroutine(EditDirectionAndSpeed());
		}
		transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("WindVector"), 0);
		if (particle != null)
		{
			particle.transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("WindVector"), 0);
		}
	}
	
    void FixedUpdate()
	{
		if (!sensors.killSwitch && sensors.earthDistance > 4f)
		{
			drone.transform.position += transform.forward * speed / 100;
		}
    }
	
	public IEnumerator EditDirectionAndSpeed()
	{
		yield return new WaitForSeconds (10f);
		transform.rotation = Quaternion.Euler(0, Random.Range(windVector - 10f, windVector + 10), 0);
		thisSpeed = Random.Range(speed - 1, speed + 1);
		StartCoroutine(EditDirectionAndSpeed());
	}
}
