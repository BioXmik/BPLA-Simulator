using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class takeDropScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "Drop")
		{
			Destroy(collision.gameObject);
		}
	}
}
