using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drop : MonoBehaviour
{
	public bool cargoIsDroped;
	public GameObject cargo;
	public GameObject dropCargo;
	public GameObject takeDrop;
	private GameObject newTakeDrop;
	public Transform takeDropContent;
	
	void Update()
	{
		//Сброс груза
		if (Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.JoystickButton3))
		{
			DropCargo();
		}
		
		//Активация эвакуационного фала
		if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.JoystickButton4))
		{
			TakeDropOnOff();
		}
	}
	
	public void TakeDropOnOff()
	{
			if (!cargoIsDroped && GetComponent<Sensors>().earthDistance > 4f)
			{
				cargoIsDroped = true;
				cargo.SetActive(false);
				newTakeDrop = Instantiate(takeDrop, takeDropContent);
				newTakeDrop.SetActive(true);
			}
			else
			{
				cargoIsDroped = false;
				Destroy(newTakeDrop);
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
			GameObject newDrop = Instantiate(dropCargo, cargo.transform.position, transform.rotation);
			newDrop.GetComponent<Rigidbody>().mass = PlayerPrefs.GetFloat("DropMass");
			yield return new WaitForSeconds (2f);
			cargoIsDroped = false;
			cargo.SetActive(true);
		}
	}
}
