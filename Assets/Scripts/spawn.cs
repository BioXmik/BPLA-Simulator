using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spawn : MonoBehaviour
{
	public GameObject[] drons;
	public GameObject[] posts;
	public Vector3 spawnPoint;
	
    void Start()
    {
		posts[PlayerPrefs.GetInt("PostsType")].SetActive(true);
        GameObject newDrone = Instantiate(drons[PlayerPrefs.GetInt("SelectDron")], spawnPoint, Quaternion.identity);
		wind Wind = GameObject.FindGameObjectWithTag("Wind").GetComponent<wind>();
		Wind.drone = newDrone;
		Wind.sensors = newDrone.GetComponent<Sensors>();
		Wind.rbDrone = newDrone.GetComponent<Rigidbody>();
	}
}
