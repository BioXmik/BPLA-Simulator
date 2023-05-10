using DroneController.Propelers;
using UnityEngine;

public class DronePropelers : ElisaScript {

	private battery batteryScript;
	private Sensors sensorsScript;
	public GameObject AnimatedGO;

	void Start()
	{
		batteryScript = GetComponent<battery>();
		sensorsScript = GetComponent<Sensors>();
	}
	
    public override void Awake()
    {
        base.Awake(); //I would suggest you to put code below this line or in a Start() method
    }

    void Update()
    {
		if (batteryScript.energy > 0 && !sensorsScript.killSwitch)
		{
			RotationInputs();
			RotationDifferentials();
		}
		else
		{
			AnimatedGO.GetComponent<Animator>().enabled = false;
		}
    }
}