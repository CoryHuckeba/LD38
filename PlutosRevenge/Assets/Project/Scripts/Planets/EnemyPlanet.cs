using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlanet : MonoBehaviour {

    #region Cached Components

    public GameObject minimapicon;

    #endregion Cached Components


    #region Properties & Variables

    public float rotationSpeed;

    #endregion Properties & Variables

    public string name;
    public string details;
    public int maxHealth;

    [HideInInspector]
    public int currentHealth;

    // Events for face managers to react to 
    public event System.Action damaged;
    public event System.Action dead;
    public event System.Action playerDamaged;
    public event System.Action playerDied;

    #region MonoBehaviour Implementation

    // Use this for initialization
    void Start ()
    {
        minimapicon.SetActive(true);
        currentHealth = maxHealth;
	}

    private void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    #endregion MonoBehaviour Implementation


    #region Public 

    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (dead != null)
                dead();
        }

        else
        {
            if (damaged != null)
                damaged();
        }
    }

    #endregion Public Interface
}
