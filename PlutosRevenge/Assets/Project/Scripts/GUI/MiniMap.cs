using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {

    #region Cached Components

    public GameObject panel;

    // Celestial bodies
    public GameObject sun;
    public GameObject mercury;
    public GameObject venus;
    public GameObject earth;
    public GameObject mars;
    public GameObject jupiter;
    public GameObject saturn;
    public GameObject uranus;
    public GameObject neptune;

    #endregion Cached Components


    #region Properties & Variables

    private bool active = true;

    #endregion Properties & Variables

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMap();
        }
	}


    #region Public Interface

    public void ToggleMap()
    {
        active = !active;
        panel.SetActive(active);
    }

    #endregion Public Interface


    #region Private Helpers



    #endregion Private Helpers
}
