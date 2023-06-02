using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
	public GameObject[] drons;
	public GameObject startUI;
	
	
	public string[] scenes;
	public Dropdown sceneDropdown;
	public Dropdown mode;
	public Dropdown postsMode;
	public Dropdown exercises;
	public Toggle changingSpeedAndDirectionWind;
	
	public Slider windVector;
	public Slider valueWind;
	public Slider dropMass;
	
	public GameObject vectorUI;
	
	void Start()
	{
		SelectDron(PlayerPrefs.GetInt ("SelectDron"));
		EditVector();
	}
	
	public void SelectDron(int dronId)
	{
		PlayerPrefs.SetInt("SelectDron", dronId);
		for (int i = 0; drons.Length > i; i++)
		{
			drons[i].SetActive(false);
		}
		drons[dronId].SetActive(true);
	}
	
	public void EditVector()
	{
		vectorUI.transform.rotation = Quaternion.Euler(0, 0, windVector.value);
	}
	
	public void Play()
	{
		startUI.SetActive(true);
	}
	
	public void StartGame()
	{
		PlayerPrefs.SetInt("Mode", mode.value);
		PlayerPrefs.SetFloat("DropMass", dropMass.value);
		PlayerPrefs.SetInt("PostsType", postsMode.value);
		PlayerPrefs.SetFloat("WindVector", windVector.value);
		PlayerPrefs.SetFloat("ValueWind", valueWind.value);
		PlayerPrefs.SetFloat("Exercises", valueWind.value);
		PlayerPrefs.SetString("ChangingSpeedAndDirectionWind", changingSpeedAndDirectionWind.isOn.ToString());
		SceneManager.LoadScene(scenes[sceneDropdown.value]);
	}
}