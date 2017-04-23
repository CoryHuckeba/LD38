using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory : Singleton<AsteroidFactory> {

    #region Cached Components

    // prefabs
    public GameObject smallAsteroidPrefab;
    public GameObject mediumAsteroidPrefab;
    public GameObject largeAsteroidPrefab;

    public GameObject cometPrefab;

    public CircleCollider2D spawnAreaTrigger;
    public Transform[] spawners;

    #endregion Cached Components


    #region Propeties & Variables

    public float spawnRadius = 40f;
    public int maxAsteroidCount = 30;
    public float minSpawnInterval = .05f;
    public float maxSpawnInterval = 1f;
    public float maxLookAhead = 20f;        // how far ahead of the player the factory will move (at max player speed)

    private int asteroidCount = 0;

    #endregion Propeties & Variables


    #region MonoBehaviour Implementation

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(SpawnRoids(0.1f));

        // Move spawners into position
        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i].transform.position -= spawners[i].right * spawnRadius;
        }
	}

    private void OnDrawGizmos()
    {
        // Debug drawing
        Gizmos.DrawSphere(transform.position, .05f);
        for (int i = 0; i < spawners.Length; i++)
        {
            Gizmos.DrawLine(spawners[i].transform.position, transform.position);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Follow ahead of player based on their speed
        Vector2 playerMovement = PlutoController.Instance.rb.velocity.normalized;
        Vector3 movement = new Vector3(playerMovement.x, playerMovement.y, 0);
        float ratio = PlutoController.Instance.speedRatio;

        transform.position = PlutoController.Instance.transform.position + movement * ratio * maxLookAhead;
	}

    #endregion MonoBehaviour Implementation


    #region Public Interface

    public void ReduceAsteroidCount()
    {
        if (asteroidCount > 0)
            asteroidCount--;        
    }

    #endregion Public Interface


    #region Private Helpers

    // Instantiates an asteroid prefab and sets its initial course
    private void SpawnAsteroid()
    {
        // Pick a random spawner
        Transform randomSpawn = spawners[Random.Range(0, spawners.Length)];

        // Decide what we're spawning
        float chance = Random.Range(0f, 1f);

        // Instantiate a prefab depending on the chance
        if (chance < 2f)
            Instantiate(largeAsteroidPrefab, randomSpawn.position, randomSpawn.rotation);        // 10% chance large
        else if (chance < .3f)
            Instantiate(mediumAsteroidPrefab, randomSpawn.position, randomSpawn.rotation);       // 20% chance medium
        else 
            Instantiate(smallAsteroidPrefab, randomSpawn.position, randomSpawn.rotation);        // 70% chance small        
    }

    // Constantly runs and spawns asteroids at random intervals
    private IEnumerator SpawnRoids(float spawnTime)
    {
        float timer = 0f;
        while (timer < spawnTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (asteroidCount < maxAsteroidCount)
            SpawnAsteroid();

        // Generate a new random interval
        float newTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        StartCoroutine(SpawnRoids(newTime));
    }

    #endregion Private Helpers
}
