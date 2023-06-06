using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Wind : MonoBehaviour 
{
	public Material[] animatedMaterials;
	private Quaternion prevRotation;
	private int windDirectionPropertyID;
	public float Factor = 1;

    private void Awake()
    {
        windDirectionPropertyID = Shader.PropertyToID ("_WindDirection");
	}

    private void Update () 
	{
		Quaternion rotation = transform.rotation;
		if (prevRotation != rotation && animatedMaterials != null) 
		{
            prevRotation = rotation;
            Vector3 dir = transform.forward * Factor;

			for (int i = 0; i < animatedMaterials.Length; i++) 
			{
                animatedMaterials[i].SetVector (windDirectionPropertyID, new Vector4(dir.x,0, dir.z));
			}
		}
	}
}