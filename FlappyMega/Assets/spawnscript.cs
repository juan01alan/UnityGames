using UnityEngine;

public class spawnscript : MonoBehaviour
{
    public GameObject objectToSpawn; // The object to spawn
    public float spawnInterval = 2f; // Time interval between spawns
    public float minRange = -1f; // Minimum range for random spawn position
    public float maxRange = 2f;  // Maximum range for random spawn position

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Start the spawning coroutine
        InvokeRepeating("SpawnObject", 0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to spawn the object
    void SpawnObject()
    {
        // Generate a random y position within the specified range
        float randomY = Random.Range(minRange, maxRange);
        // Create a new position with the current x and the random y
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, transform.position.z);

        // Instantiate the object at the current position with no rotation
        Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    }
}
