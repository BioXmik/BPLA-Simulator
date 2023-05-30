using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour
{
	public bool cargoIsDroped;
	public GameObject cargo;
	public GameObject dropCargo;
	public GameObject takeDrop;
	
	public void TakeDropOnOff()
	{
		if (!cargoIsDroped)
		{
			cargoIsDroped = true;
			cargo.SetActive(false);
			takeDrop.SetActive(true);
		}
		else
		{
			cargoIsDroped = false;
			takeDrop.SetActive(false);
			cargo.SetActive(true);
		}
	}
	
	public void DropCargo()
	{
		StartCoroutine(DropCargoCoroutine());
	}
	
	public IEnumerator DropCargoCoroutine()
	{
		if (!GetComponent<Sensors>().killSwitch && GetComponent<battery>().energy > 0 && !cargoIsDroped)
		{
			cargoIsDroped = true;
			cargo.SetActive(false);
			GameObject newDrop = Instantiate(dropCargo, new Vector3(transform.position.x, transform.position.y -0.5f, transform.position.z), transform.rotation);
			newDrop.GetComponent<Rigidbody>().mass = PlayerPrefs.GetFloat("DropMass");
			yield return new WaitForSeconds (2f);
			cargoIsDroped = false;
			cargo.SetActive(true);
		}
	}
}
