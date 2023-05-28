using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour
{
	public bool cargoIsDroped;
	public GameObject cargo;
	public GameObject dropCargo;
	
	public void DropCargo()
	{
		if (!GetComponent<Sensors>().killSwitch && GetComponent<battery>().energy > 0)
		{
			if (!cargoIsDroped)
			{
				cargoIsDroped = true;
				cargo.SetActive(false);
				GameObject newDrop = Instantiate(dropCargo, new Vector3(transform.position.x, transform.position.y -1f, transform.position.z), Quaternion.identity);
				newDrop.GetComponent<Rigidbody>().mass = PlayerPrefs.GetFloat("DropMass");
			}
		}
	}
}
