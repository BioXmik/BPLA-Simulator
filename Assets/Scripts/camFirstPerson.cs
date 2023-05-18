using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFirstPerson : MonoBehaviour
{
	public Transform drone;
	
	void Start()
	{
		drone = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
    void Update()
    {
        Vector3 diference = drone.transform.position - transform.position;
		transform.rotation = Quaternion.LookRotation(diference);
    }
}