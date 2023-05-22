using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
	private float speed;
	
	public Rigidbody rbDrone;
	public GameObject drone;
	
	public GameObject particle;
	
	void Start()
	{
		transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("WindVector"), 0);
		if (particle != null)
		{
			particle.transform.rotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("WindVector"), 0);
		}
		speed = PlayerPrefs.GetFloat("ValueWind") * -1;
	}
	
    public void WindVelocity()
	{
		if (drone.GetComponent<Sensors>().mode != "auto" && drone.GetComponent<Sensors>().mode != "loiter" && drone.GetComponent<Sensors>().mode != "althold")
		{
			drone.transform.position += transform.forward * speed;
		}
    }
}
