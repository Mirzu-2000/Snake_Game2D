
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject speedUpPrefab;    // Assign in inspector
    public GameObject scoreBoostPrefab; // Assign in inspector
    public GameObject shieldPrefab;     // Assign in inspector

    public float minSpawnInterval = 5.0f;  // Minimum interval for spawning power-ups
    public float maxSpawnInterval = 10.0f;  // Maximum interval for spawning power-ups
    private float spawnTimer = 0.0f;      // Timer to track the spawn interval
    private float currentSpawnInterval;   // Current spawn interval for the next power-up

    private GameObject currentPowerUp;    // Reference to the currently active power-up

    private int spawnRangeX = 17;         // X boundary for spawn range
    private int spawnRangeYP = 7;         // Positive Y boundary
    private int spawnRangeYN = -9;        // Negative Y boundary

    void Start()
    {
        // Initialize the spawn interval with a random value within the range
        SetRandomSpawnInterval();
    }

    void Update()
    {
        // Update the spawn timer
        spawnTimer += Time.deltaTime;

        // Check if the spawn timer has reached the interval and no power-up is active
        if (spawnTimer >= currentSpawnInterval && currentPowerUp == null)
        {
            SpawnPowerUp();  // Spawn a new power-up
            spawnTimer = 0f; // Reset the timer

            // Set a new random spawn interval for the next power-up
            SetRandomSpawnInterval();
        }
    }

    // Method to spawn a random power-up
    private void SpawnPowerUp()
    {
        // Randomly choose which power-up to spawn
        GameObject powerUpPrefab = ChooseRandomPowerUp();

        // Generate a random position within the specified spawn boundaries
        Vector2 spawnPosition = new Vector2(
            Random.Range(-spawnRangeX, spawnRangeX),
            Random.Range(spawnRangeYN, spawnRangeYP)
        );

        // Instantiate the selected power-up prefab at the random position
        currentPowerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);

        // Destroy the power-up after 1.5 seconds if not collected
        Destroy(currentPowerUp, 4f);
    }

    // Method to randomly select a power-up
    private GameObject ChooseRandomPowerUp()
    {
        // Randomly pick between SpeedUp, ScoreBoost, and Shield
        int randomChoice = Random.Range(0, 3);

        switch (randomChoice)
        {
            case 0:
                return speedUpPrefab;   // SpeedUp
            case 1:
                return scoreBoostPrefab; // ScoreBoost
            case 2:
                return shieldPrefab;     // Shield
            default:
                return speedUpPrefab;    // Default (should not happen)
        }
    }

    // Method to set a random spawn interval
    private void SetRandomSpawnInterval()
    {
        // Randomly set the spawn interval between min and max values
        currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }
}
