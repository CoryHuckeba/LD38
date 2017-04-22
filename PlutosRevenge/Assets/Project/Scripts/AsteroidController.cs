using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    #region Cached Components

    Rigidbody2D rb;

    #endregion Cached Components


    #region Properties & Variables

    public float influenceDistance;     // How close the asteroid has to be before being affected by gravity
    public float damage;
    public float mass;
    public float gravityDampRange;      // Once the asteroid is this close the gravitational effects eaken (should help orbits?)

    private float influenced;           // Ratio (0-1) of how much influence Pluto's gravity has on this asteroid

    #endregion Properties & Variables


    #region MonoBehaviour Implementation

    void Start ()
    {
        // Cache my RigidBody
        rb = gameObject.GetComponent<Rigidbody2D>();

		// Debug: For now assign some random force?
	}
	
	void Update ()
    {
        // Check if we've been influenced by the player's gravity
        float gravity = PlutoController.Instance.gravity;

        // If the player is exerting gravity and we're close enough to be affected
        if (gravity > 0)
        {
            // Determine if we're close enough to the player
            Vector3 angleToPluto = PlutoController.Instance.transform.position - transform.position;
            float distanceToPluto = Mathf.Abs(angleToPluto.magnitude);

            if (distanceToPluto < influenceDistance)
            {
                // Get damp ratio
                float dampRatio = Mathf.Min(distanceToPluto / gravityDampRange, 1f);

                // Apply gravitational force to our momentum
                rb.AddForce(angleToPluto.normalized * gravity * dampRatio);
            }
        }
	}

    #endregion MonoBehaviour Implementation
}
