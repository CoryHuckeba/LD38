using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoController : Singleton<PlutoController>
{
    #region Properties & Variables

    // Movement Params
    public float speed;
    public float turnSpeed; // Is this going to be used?
    public float gravity;
    public float distanceFromCamera = 10f;

    #endregion Properties & Variables


    #region MonoBehaviour Implementation
        
    void Start ()
    {
		
	}
	
	// Get player Inputs
	void Update ()
    {
        // Calculate and set player's facing direction
        SetFacingDirection();

        // Movement (Left Mouse)
        if (Input.GetMouseButtonDown(0))
        {
            // TODO
        }

        // Gravity (Right Mouse)
        if (Input.GetMouseButtonDown(1))
        {
            // TODO
        }
	}

    #endregion MonoBehaviour Implementation


    #region Private Helpers

    // Takes the current mouse position and converts it into a direction for pluto to face
    private void SetFacingDirection()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distanceFromCamera; 
        Vector3 myPosition = Camera.main.WorldToScreenPoint(transform.position);
        mousePosition.x = mousePosition.x - myPosition.x;
        mousePosition.y = mousePosition.y - myPosition.y;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));        
    }

    #endregion Private Helpers
}
