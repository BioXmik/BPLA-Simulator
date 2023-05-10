using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
	public GameObject[] drons;
	public GameObject[] posts;
	
    void Start()
    {
		posts[PlayerPrefs.GetInt("PostsType")].SetActive(true);
        GameObject newDrone = Instantiate(drons[PlayerPrefs.GetInt("SelectDron")], new Vector3(600, 50, 500), Quaternion.identity);
		GameObject.FindGameObjectWithTag("Wind").GetComponent<wind>().drone = newDrone;
		GameObject.FindGameObjectWithTag("Wind").GetComponent<wind>().rbDrone = newDrone.GetComponent<Rigidbody>();
	}
}
