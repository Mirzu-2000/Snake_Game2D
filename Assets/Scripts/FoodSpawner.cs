using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject massGainerPrefab; // Assign in inspector
    public GameObject massBurnerPrefab; // Assign in inspector
    public float spawnInterval = 2.5f; // Time to despawn food if uneaten
    private GameObject currentFood;
    private int spwanRangeX = 17;
    private int spwanRangeYP = 7;
    private int spwanRangeYN = -9;


    private float spawnTimer;

    void Update()
    {
        // Check if there is no active food and if enough time has passed to spawn
        spawnTimer += Time.deltaTime;
        if (currentFood == null && spawnTimer >= spawnInterval)
        {
            SpawnFood();
            spawnTimer = 0f;
        }
    }

    private void SpawnFood()
    {
        // Randomly choose between Mass Gainer and Mass Burner
        GameObject foodPrefab = Random.value > 0.5f ? massGainerPrefab : massBurnerPrefab;

        // Generate a random position within specified boundaries
        Vector2 spawnPosition = new Vector2(
            Random.Range(-spwanRangeX, spwanRangeX),
            Random.Range(spwanRangeYN, spwanRangeYP)
        );

        // Instantiate the selected food prefab at the spawn position
        currentFood = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);

        // Destroy food after the spawnInterval if uneaten
        Destroy(currentFood, spawnInterval);
    }
}
