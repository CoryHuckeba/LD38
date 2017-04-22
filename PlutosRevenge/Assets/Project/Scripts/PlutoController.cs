using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoController : Singleton<PlutoController>
{
    #region Cached Components

    public Rigidbody2D rb;
    public CircleCollider2D col;

    #endregion Cached Components

    #region Properties & Variables

    // Movement Params
    public float speed;
    public float gravityForce;
    public float gravityBuildTime;              // TIme to hit max gravity in seconds
    public float distanceFromCamera = 10f;

    // Publicly exposed gravity
    [HideInInspector] public float gravity = 0f;

    #endregion Properties & Variables


    #region MonoBehaviour Implementation
        
    void Start ()
    {
		
	}

    void FixedUpdate()
    {
        // Movement (Left Mouse)
        if (Input.GetMouseButton(0))
        {
            // Apply momentum in that direction
            rb.AddForce(transform.right * speed);
        }
    }

	// Get player Inputs
	void Update ()
    {
        // Calculate and set player's facing direction
        SetFacingDirection();
        
        // Gravity (Right Mouse)
        if (Input.GetMouseButton(1))
        {
            if (gravity < gravityForce)
            {
                gravity = Mathf.Min(gravityForce, gravity + (gravityForce * Time.deltaTime / gravityBuildTime));
            }
        }
        else if (gravity > 0)
        {
            gravity = 0;
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
