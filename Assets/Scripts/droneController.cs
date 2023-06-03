using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneController : MonoBehaviour
{
	public float speed = 2f;
	
	private InputControll _input;
	private Rigidbody rb;
	
	private void Awake()
	{
		_input  = new InputControll();
	}
	
	private void OnEnable()
	{
		_input.Enable();
	}
	
	private void OnDisable()
	{
		_input.Disable();
	}
	
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	void Update()
	{
		Vector3 direction = _input.Drone.Movement.ReadValue<Vector3>();
		
		Move(direction);
	}
	
	private void Move(Vector3 direction)
	{
		transform.position += direction * speed * Time.deltaTime;
	}
}