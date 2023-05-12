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
		GameObject.FindGameObjectWithTag("Wind").GetComponent<wind>().drone = newDrone;
		GameObject.FindGameObjectWithTag("Wind").GetComponent<wind>().rbDrone = newDrone.GetComponent<Rigidbody>();
	}
}
