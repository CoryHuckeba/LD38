using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image fillBar; 

	// Use this for initialization
	void Start ()
    {
        fillBar.fillAmount = 1f;

        // Register to player's health change event
        PlutoController.Instance.healthChanged += UpdateHealth;
	}
	
	private void UpdateHealth(int newHP)
    {
        Debug.Log(" A THING HAPPENED: " + (float)newHP / PlutoController.Instance.maxhealth);
        fillBar.fillAmount = (float)newHP / PlutoController.Instance.maxhealth;
    }
}
