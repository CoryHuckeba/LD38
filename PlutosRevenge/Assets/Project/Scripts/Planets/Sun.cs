using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    public float mercuryRotationSpeed;
    public float venusRotationSpeed;
    public float earthRotationSpeed;

    // Centerpoints to do orbits
    public GameObject mercuryCenter;
    public GameObject venusCenter;
    public GameObject earthCenter;

    // Update is called once per frame
    void FixedUpdate ()
    {
        // Mercury
        Vector3 rotation = Vector3.RotateTowards(mercuryCenter.transform.right, mercuryCenter.transform.up, mercuryRotationSpeed * Time.deltaTime * Mathf.Deg2Rad, 1);
        mercuryCenter.transform.Rotate(Vector3.forward, Time.deltaTime * mercuryRotationSpeed);

        // Venus
	}
}
