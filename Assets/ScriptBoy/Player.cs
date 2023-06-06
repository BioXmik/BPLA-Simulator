using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	public float moveSpeed = 3;
	public float mouseSensitivity = 120;

	// Use this for initialization
	void Start () 
	{
		#if UNITY_EDITOR

		#else
		Cursor.visible = false;
		#endif
	}
	
	// Update is called once per frame
	void Update () 
	{

		float shift = Input.GetKey(KeyCode.LeftShift)? 2 * Time.deltaTime : Time.deltaTime;

		if (Input.GetKey (KeyCode.W)) 
		{
			transform.Translate (0,0,moveSpeed * shift );
		}

		if (Input.GetKey (KeyCode.S)) 
		{
			transform.Translate (0,0,-moveSpeed * shift );
		}

		if (Input.GetKey (KeyCode.D)) 
		{
			transform.Translate (moveSpeed * shift ,0,0);
		}

		if (Input.GetKey (KeyCode.A)) 
		{
			transform.Translate (-moveSpeed * shift ,0,0);
		} 

		if (Input.GetKey (KeyCode.E)) 
		{
			transform.Translate (0,moveSpeed * shift ,0);
		} 

		if (Input.GetKey (KeyCode.Q)) 
		{
			transform.Translate (0,-moveSpeed * shift ,0);
		} 

		Vector3 r = transform.eulerAngles;
		r.x -= Input.GetAxisRaw ("Mouse Y") * mouseSensitivity * Time.deltaTime;
		r.y += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
		r.z = 0;
		transform.eulerAngles = r;
	}
}
