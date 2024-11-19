using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject massGainerPrefab; // Assign in inspector
    public GameObject massBurnerPrefab; // Assign in inspector

    public float minSpawnInterval = 1.5f; // Minimum time for food to respawn
    public float maxSpawnInterval = 3.5f; // Maximum time for food to respawn
    private float currentSpawnInterval;   // Randomized time for the next spawn

    private GameObject currentFood;
    private int spawnRangeX = 17;          // X boundary for spawn range
    private int spawnRangeYP = 7;          // Positive Y boundary
    private int spawnRangeYN = -9;         // Negative Y boundary

    private float spawnTimer;

    void Start()
    {
        // Set an initial random spawn interval
        SetRandomSpawnInterval();
    }

    void Update()
    {
        // Update the spawn timer
        spawnTimer += Time.deltaTime;

        // Check if there is no active food and if enough time has passed to spawn
        if (currentFood == null && spawnTimer >= currentSpawnInterval)
        {
            SpawnFood();
            spawnTimer = 0f; // Reset the timer

            // Set a new random spawn interval for the next food spawn
            SetRandomSpawnInterval();
        }
    }

    // Method to spawn food at a random position
    private void SpawnFood()
    {
        // Randomly choose between Mass Gainer and Mass Burner
        GameObject foodPrefab = Random.value > 0.5f ? massGainerPrefab : massBurnerPrefab;

        // Generate a random position within the specified boundaries
        Vector2 spawnPosition = new Vector2(
            Random.Range(-spawnRangeX, spawnRangeX),
            Random.Range(spawnRangeYN, spawnRangeYP)
        );

        // Instantiate the selected food prefab at the spawn position
        currentFood = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);

        // Destroy food after the spawn interval if uneaten
        Destroy(currentFood, currentSpawnInterval);
    }

    // Method to set a new random spawn interval
    private void SetRandomSpawnInterval()
    {
        // Randomly choose a spawn interval between the minimum and maximum values
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
