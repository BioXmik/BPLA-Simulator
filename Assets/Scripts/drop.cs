using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour
{
	public GameObject dropCargo;
	
	public void DropCargo()
	{
		if (!GetComponent<Sensors>().killSwitch && GetComponent<battery>().energy > 0 && PlayerPrefs.GetFloat("Exercises") == 1)
		{
			GameObject newDrop = Instantiate(dropCargo, new Vector3(transform.position.x, transform.position.y -2f, transform.position.z), Quaternion.identity);
			newDrop.GetComponent<Rigidbody>().mass = PlayerPrefs.GetFloat("DropMass");
		}
	}
}
