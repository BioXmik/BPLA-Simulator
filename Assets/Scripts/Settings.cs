using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	public GameObject SettingsUI;
	public Dropdown GraphicDropdown;
    private int graphic;
	
	void Start()
	{
		if (!PlayerPrefs.HasKey("Graphics")) {PlayerPrefs.SetInt("Graphics",  5);}
		graphic = PlayerPrefs.GetInt("Graphics");
		GraphicDropdown.value = graphic;
		QualitySettings.SetQualityLevel(graphic);
	}
	
	public void EditGraphic()
    {
		QualitySettings.SetQualityLevel(GraphicDropdown.value);
		PlayerPrefs.SetInt("Graphics", GraphicDropdown.value);
    }
	
	public void SettingOnOff(bool activeStatus)
	{
		SettingsUI.SetActive(activeStatus);
	}
}