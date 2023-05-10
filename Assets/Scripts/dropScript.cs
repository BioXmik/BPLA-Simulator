using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropScript : MonoBehaviour
{
	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Wire")
		{
			GetComponent<Rigidbody>().isKinematic = true;
			transform.position = new Vector3 (transform.position.x, transform.position.y - 0.5f, transform.position.z);
		}
	}
}
