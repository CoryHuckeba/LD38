using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Struct to contain information for each of the asteroid sizes
[System.Serializable]
public struct AsteroidStats
{
    public float mass;
    public int damage;
    public float startingVelocity;
}

public enum AsteroidSize
{
    Small = 1,
    Medium = 2,
    Large = 3
}

public class AsteroidController : MonoBehaviour {

    #region Cached Components

    Rigidbody2D rb;

    #endregion Cached Components


    #region Properties & Variables    

    public AsteroidSize size;
    public AsteroidStats stats;

    public float minimumGravityRatio;   // The lowest effectiveness the gravitational effect can be
    public float maxGravityRange;       // Once the asteroid is this close the gravitational effects hit max (should help orbits?)
    public float maxSpeed;              // Top orbital speed
    public float acceleration;          // How quickly speed increases when influenced

    public float randomAngle;           // Once spawned the direction the asteroid is facing will be randomly changed within this range (left and right)

    private float influenced;           // Ratio (0-1) of how much influence Pluto's gravity has on this asteroid
    private float distanceToPluto = 99f;
    private float angleToPluto = 0f;

    #endregion Properties & Variables


    #region MonoBehaviour Implementation

    void Start ()
    {
        // Cache my RigidBody
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(transform.right.x, transform.right.y) * stats.startingVelocity;
    }
	
	void Update ()
    {
        // Check if we've been influenced by the player's gravity
        float gravity = PlutoController.Instance.gravity;

        // If the player is exerting gravity and we're close enough to be affected
        if (gravity > 0)
        {
            // Set the distance to the player
            Vector3 vectorToPluto = PlutoController.Instance.transform.position - transform.position;
            angleToPluto = Vector2.Angle(new Vector2(vectorToPluto.x, vectorToPluto.y), rb.velocity);
            distanceToPluto = Mathf.Abs(vectorToPluto.magnitude);

            // Determine if we're close enough to the player
            if (distanceToPluto < PlutoController.Instance.influence)
            {
                // Calculate the effectiveness, then cap it at the minimum
                float ratio = (maxGravityRange / distanceToPluto);
                if (ratio < minimumGravityRatio) ratio = minimumGravityRatio;

                // Apply gravitational force to our momentum
                rb.AddForce(vectorToPluto.normalized * gravity * ratio);
            }
        }
	}

    private void LateUpdate()
    {
        // Apply more speed while we're in gravitational pull (if not at max)
        //if (distanceToPluto < (PlutoController.Instance.influence / 2) && rb.velocity.magnitude < maxSpeed)
        //{
        //    float spd = rb.velocity.magnitude;
        //    float rat = angleToPluto / 180;

        //    Debug.Log("Angle to Pluto: " + angleToPluto + ", " + "Rat: " + rat);

        //    spd = Mathf.Min(spd + (acceleration * Time.deltaTime) * rat, maxSpeed);
        //    rb.velocity = rb.velocity.normalized * spd;
        //}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If this was another asteroid reduce our size by one    
        if (collision.gameObject.tag == "Pluto")
        {
            PlutoController.Instance.AdjustHealth(stats.damage * -1);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If this was the asteroid spawne destroy self
        if (collision.tag == "AsteroidFactory")
        {
            AsteroidFactory.Instance.ReduceAsteroidCount();
            Destroy(gameObject);
        }
    }

    #endregion MonoBehaviour Implementation
}
