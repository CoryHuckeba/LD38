using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    #region Cached Components

    Rigidbody2D rb;

    #endregion Cached Components


    #region Properties & Variables
    
    public float damage;
    public float mass;

    public float minimumGravityRatio;  // The lowest effectiveness the gravitational effect can be
    public float maxGravityRange;       // Once the asteroid is this close the gravitational effects hit max (should help orbits?)

    private float influenced;           // Ratio (0-1) of how much influence Pluto's gravity has on this asteroid

    #endregion Properties & Variables


    #region MonoBehaviour Implementation

    void Start ()
    {
        // Cache my RigidBody
        rb = gameObject.GetComponent<Rigidbody2D>();        
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

            if (distanceToPluto < PlutoController.Instance.influence)
            {
                // Calculate the effectiveness, then cap it at the minimum
                float ratio = Mathf.Min(maxGravityRange / distanceToPluto, 1f);
                if (ratio < minimumGravityRatio) ratio = minimumGravityRatio;

                // Apply gravitational force to our momentum
                rb.AddForce(angleToPluto.normalized * gravity * ratio);
            }
        }
	}

    #endregion MonoBehaviour Implementation
}
