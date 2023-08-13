using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class docking : MonoBehaviour
{
    private InputControl inputControl;
    private GameObject drone;
    private GameObject oldDrone;
    private Rigidbody rb;
    private GameObject cameraPoint;

    private void Awake()
    {
        inputControl = new InputControl();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    void Start()
    {
        if (transform.parent.parent != null)
        {
            drone = transform.parent.parent.gameObject;
            rb = drone.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.GetComponent<PathCreator>() && !drone.GetComponent<PathFollower>())
		{
            drone.gameObject.SetActive(false);
            GameObject newDrone = Instantiate(drone.transform.GetChild(5).gameObject, drone.transform.position, Quaternion.identity);
            newDrone.transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
            docking newDroneDocking = newDrone.transform.GetChild(1).gameObject.GetComponent<docking>();
            newDroneDocking.drone = newDrone;
            newDroneDocking.oldDrone = transform.parent.parent.gameObject;
            newDroneDocking.cameraPoint = GameObject.Find("Null Rotation Object");
            GameObject.Find("Camera Mode Manager").GetComponent<cameraMode>().droneBody = newDrone.transform;
            newDrone.AddComponent<PathFollower>();
            PathFollower pathFollower = newDrone.GetComponent<PathFollower>();
            pathFollower.pathCreator = collision.gameObject.GetComponent<PathCreator>();
            pathFollower.speed = 2;
            pathFollower.endOfPathInstruction = EndOfPathInstruction.Reverse;
		}
	}

    void Update()
    {
        if (cameraPoint)
        {
            cameraPoint.transform.position = transform.position;
        }
        Vector3 direction = inputControl.Drone.Move.ReadValue<Vector3>();
        if (direction.y > 0.2f && drone.GetComponent<PathFollower>())
        {
            Disconnect();
        }
    }

    public void Disconnect()
    {
        PathFollower pathFollower = drone.GetComponent<PathFollower>();
        oldDrone.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        GameObject.Find("Camera Mode Manager").GetComponent<cameraMode>().droneBody = oldDrone.transform;
        Destroy(transform.parent.gameObject);
        oldDrone.SetActive(true);
        oldDrone.GetComponent<Sensors>().killSwitch = false;
    }
}