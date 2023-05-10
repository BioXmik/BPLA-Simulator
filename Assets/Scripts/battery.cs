using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battery : MonoBehaviour
{
	public float maxEnergy;
	public float energy;
	public float consumption;
	public float procentEnergy;
	
	public Text procentEnergyText;
	public Text consumptionText;
	
	public GameObject[] visualBattery;
	
	void Start()
	{
		procentEnergy = maxEnergy / 100;
		energy = maxEnergy;
	}
	
	void Update()
	{
		if (energy > 0)
		{
			energy = energy - consumption;
		}
		
		procentEnergyText.text = Math.Round(energy / procentEnergy).ToString() + "%";
		consumptionText.text = consumption.ToString() + "V";
		if (energy > procentEnergy * 80) {visualBattery[0].SetActive(true);} else {visualBattery[0].SetActive(false);}
		if (energy > procentEnergy * 60) {visualBattery[1].SetActive(true);} else {visualBattery[1].SetActive(false);}
		if (energy > procentEnergy * 40) {visualBattery[2].SetActive(true);} else {visualBattery[2].SetActive(false);}
		if (energy > procentEnergy * 20) {visualBattery[3].SetActive(true);} else {visualBattery[3].SetActive(false);}
		if (energy > 0) {visualBattery[4].SetActive(true);} else {visualBattery[4].SetActive(false);}
	}
}
