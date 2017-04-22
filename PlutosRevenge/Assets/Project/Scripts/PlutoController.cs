//#define MOUSE_MOVEMENT
#define KEY_MOVEMENT

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
    public float influence;                     // How far away objects will be afected by gravity

    // Speed tracking params
    public float maxSpeed;
    public float speedRatio = 0f;

    // Publicly exposed gravity
    [HideInInspector] public float gravity = 0f;

    // Key normal vectors (to prevent instantiating new Vector3s every frame
    Vector3 RIGHT = new Vector3(1, 0, 0);
    Vector3 LEFT = new Vector3(-1, 0, 0);
    Vector3 UP = new Vector3(0, 1, 0);
    Vector3 DOWN = new Vector3(0, -1, 0);

    #endregion Properties & Variables


    #region MonoBehaviour Implementation

    void Start ()
    {
		
	}

    void FixedUpdate()
    {
        #if MOUSE_MOVEMENT
        // Movement (Left Mouse)
        if (Input.GetMouseButton(0))
        {
            // Apply momentum in that direction
            rb.AddForce(transform.right * speed);

            // Cap speed at maximum
            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = rb.velocity.normalized * maxSpeed;

            // Update speed ratio
            speedRatio = rb.velocity.magnitude/maxSpeed;
        }
        #elif KEY_MOVEMENT


        Vector3 keyVelocity = Vector3.zero;

        // Directional force
        if (Input.GetKey(KeyCode.A))
            keyVelocity += LEFT;
        if (Input.GetKey(KeyCode.D))
            keyVelocity += RIGHT;
        if (Input.GetKey(KeyCode.W))
            keyVelocity += UP;
        if (Input.GetKey(KeyCode.S))
            keyVelocity += DOWN;

        // Apply momentum in that direction
        rb.AddForce(keyVelocity.normalized * speed);

        // Cap speed at maximum
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        // Update speed ratio
        speedRatio = rb.velocity.magnitude / maxSpeed;

        #endif
    }

	// Get player Inputs
	void Update ()
    {
        #if MOUSE_MOVEMENT
        // Calculate and set player's facing direction
        SetFacingDirection();
        #endif
        
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
