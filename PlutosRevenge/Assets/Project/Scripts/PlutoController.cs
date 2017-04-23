//#define MOUSE_MOVEMENT
#define KEY_MOVEMENT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Used to track the state of Pluto's faces. The numebrs represent priority, higher number = higher prio.
/// </summary>
public enum PlutoFaces
{
    Default =       0,
    Gravity =       1,
    MovingFast =    2,
    Hurt =          3,
}

public class PlutoController : Singleton<PlutoController>
{
    #region Cached Components

    public Rigidbody2D rb;
    public CircleCollider2D col;
    public GameObject minimapIcon;

    public SpriteRenderer faceSprite;

    #endregion Cached Components

    #region Properties & Variables

    // Movement Params
    public float speed;
    public float gravityForce;
    public float gravityBuildTime;              // TIme to hit max gravity in seconds
    public float distanceFromCamera = 10f;
    public float influence;                     // How far away objects will be afected by gravity

    private bool boosting = false;
    private float boost;

    // Speed tracking params
    public float maxSpeed;
    public float speedRatio = 0f;

    // Stats
    public int maxhealth = 500;
    public float maxBoost = 3f;
    public float boostSpeed = 120f;

    private int health;

    // Publicly exposed gravity
    [HideInInspector] public float gravity = 0f;

    // Key normal vectors (to prevent instantiating new Vector3s every frame
    Vector3 RIGHT = new Vector3(1, 0, 0);
    Vector3 LEFT = new Vector3(-1, 0, 0);
    Vector3 UP = new Vector3(0, 1, 0);
    Vector3 DOWN = new Vector3(0, -1, 0);

    // Face state values
    public float hurtFaceTime = 1f;
    
    private bool fastFace = false;
    private bool hurtFace = false;
    private bool gravityFace = false;

    // Events
    public event System.Action<int> healthChanged;

    #endregion Properties & Variables


    #region MonoBehaviour Implementation

    void Start ()
    {
        minimapIcon.SetActive(true);

        // Set tracking variables to initial values
        health = maxhealth;
        boost = maxBoost;
    }

    void FixedUpdate()
    {
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
        if (!boosting)
            rb.AddForce(keyVelocity.normalized * speed);
        else
            rb.AddForce(keyVelocity.normalized * speed * 2);

        // Cap speed at maximum
        if (!boosting && rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;
        else if (boosting && rb.velocity.magnitude > boostSpeed)
            rb.velocity = rb.velocity.normalized * boostSpeed;

        // Update speed ratio
        speedRatio = rb.velocity.magnitude / maxSpeed;
    }

	// Get player Inputs
	void Update ()
    {
        // Gravity (Left Mouse)
        if (Input.GetMouseButton(0))
        {
            gravityFace = true;

            if (gravity < gravityForce)
            {
                gravity = Mathf.Min(gravityForce, gravity + (gravityForce * Time.deltaTime / gravityBuildTime));
            }
        }
        else if (gravity > 0)
        {
            gravityFace = false;
            gravity = 0;
        }
        else
            gravityFace = false;

        // Boost
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!boosting)
                boosting = true;

            boost = Mathf.Max(boost - Time.deltaTime, 0f);

            if (boost == 0f)
                boosting = false;
        }
        else if (boost < maxBoost)
        {
            if (boosting)
                boosting = false;

            boost = Mathf.Min(boost += Time.deltaTime, maxBoost);
        }

        // Boost screenshake start
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            fastFace = true;
            // TODO
        }

        // Boost screenshake end
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            fastFace = false;
            // TODO
        }

        // Set the proper face yo
        SetCurrentFace();
    }

    #endregion MonoBehaviour Implementation


    #region Public Interface

    public void AdjustHealth(int amount)
    {
        health += amount;

        if (healthChanged != null)
            healthChanged(health);

        StartCoroutine(HurtFace());
    }

    #endregion Public Interface


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

    private void SetCurrentFace()
    {
        if (hurtFace)
            FaceManager.Instance.SetFace(PlutoFaces.Hurt);
        else if (gravityFace)
            FaceManager.Instance.SetFace(PlutoFaces.Gravity);
        else if (fastFace)
            FaceManager.Instance.SetFace(PlutoFaces.MovingFast);
        else
            FaceManager.Instance.SetFace(PlutoFaces.Default);
    }

    private IEnumerator HurtFace()
    {
        hurtFace = true;

        float time = 0f;
        while (time < hurtFaceTime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        hurtFace = false;
    }

    #endregion Private Helpers
}
