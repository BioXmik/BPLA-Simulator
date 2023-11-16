using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFirstPerson : MonoBehaviour
{
	public Transform drone;
	
    void Update()
    {
        if (drone)
        {
            Vector3 diference = drone.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(diference);
        }
    }
}
