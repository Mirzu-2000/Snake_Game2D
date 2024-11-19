using UnityEngine;
using System.Collections.Generic;

public class SnakeTwoController : MonoBehaviour
{

    // Movement settings
    public float gridSize = 1.0f; // Size of one grid unit for snake movement
    public float moveInterval = 0.3f; // Time in seconds between each move
    private float moveTimer = 0.0f; // Timer to control movement intervals
    private Vector2 moveDirection = Vector2.left; // Starting direction of movement
    private bool canChangeDirection = true; // Prevent immediate direction reversal

    // Body management
    public GameObject bodySegmentPrefab; // Prefab for the snake's body segments
    private List<Transform> bodySegments = new List<Transform>(); // List to track body segments

    // Scoring and game logic
    public TwoPlayerScoreManager scoreManager; // Reference to the ScoreManager script for handling scores



    // Power-up related properties
    private bool isSpeedBoosted = false; // Tracks if SpeedUp power-up is active
    private bool isScoreBoosted = false; // Tracks if ScoreBoost power-up is active
    private bool isShieldActive = false;  // Track if Shield power-up is active
    private float originalMoveInterval; // Stores original move interval for resetting after SpeedUp
    private float powerUpDuration = 4f; // Duration for which a power-up effect is active

    void Start()
    {
        // Save the original move interval for resetting after SpeedUp power-up
        originalMoveInterval = moveInterval;

        // Add the initial body segment at a position behind the head
        AddInitialBodySegment();
    }

    void Update()
    {

        moveTimer += Time.deltaTime;

        if (moveTimer >= moveInterval)
        {
            Move();
            moveTimer = 0.0f; // Reset the timer after moving
        }

        HandleInput(); // Ensure input can be handled outside of the timer

    }

    private void HandleInput()
    {
        if (canChangeDirection)
        {
            if (Input.GetKeyDown(KeyCode.W) && moveDirection != Vector2.down) moveDirection = Vector2.up;
            else if (Input.GetKeyDown(KeyCode.S) && moveDirection != Vector2.up) moveDirection = Vector2.down;
            else if (Input.GetKeyDown(KeyCode.A) && moveDirection != Vector2.right) moveDirection = Vector2.left;
            else if (Input.GetKeyDown(KeyCode.D) && moveDirection != Vector2.left) moveDirection = Vector2.right;

            // canChangeDirection = false; // Prevent immediate reversals
        }
    }

    private void Move()
    {

        Vector3 previousPosition = transform.position;

        // Move the head in increments of grid size
        transform.position += (Vector3)moveDirection * gridSize;

        // Move body segments
        for (int i = 0; i < bodySegments.Count; i++)
        {
            Vector3 tempPosition = bodySegments[i].position;
            bodySegments[i].position = previousPosition;
            previousPosition = tempPosition;
        }

        canChangeDirection = true; // Allow direction change again after moving

        if (!isShieldActive && CheckSelfCollision())
        {
            Die();
        }
    }

    private bool CheckSelfCollision()
    {
        for (int i = 0; i < bodySegments.Count; i++)
        {
            if (transform.position == bodySegments[i].position)
            {
                return true;
            }
        }
        return false;
    }

    private void AddInitialBodySegment()
    {
        // Calculate the initial position for the first body segment
        Vector3 initialSegmentPosition = transform.position - (Vector3)moveDirection * gridSize;

        // Instantiate the body segment prefab
        Transform newSegment = Instantiate(bodySegmentPrefab).transform;

        // Set its position to the calculated position
        newSegment.position = initialSegmentPosition;

        // Add the new segment to the list of body segments
        bodySegments.Add(newSegment);
    }

    public void AddBodySegment()
    {
        // Instantiate a new body segment prefab
        Transform newSegment = Instantiate(bodySegmentPrefab).transform;

        // Position it based on the last segment's position, or at the head's position for the first segment
        if (bodySegments.Count > 0)
        {
            newSegment.position = bodySegments[bodySegments.Count - 1].position; // Position it behind the last segment
        }
        else
        {
            newSegment.position = transform.position; // Position it at the head if it's the first segment
        }

        // Add the new segment to the list of segments
        bodySegments.Add(newSegment);
    }

    private void RemoveBodySegment()
    {
        // Ensure there is at least one segment to remove
        if (bodySegments.Count > 0)
        {
            // Get the last segment in the list
            Transform segmentToRemove = bodySegments[bodySegments.Count - 1];

            // Remove it from the list
            bodySegments.RemoveAt(bodySegments.Count - 1);

            // Destroy the segment GameObject to remove it visually from the game
            Destroy(segmentToRemove.gameObject);
        }
    }

    private void Die()
    {
        Debug.Log("Game Over! Snake collided with its body.");
    }


    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("MassGainer"))
        {
            AddBodySegment(); // Adds a body segment to the snake
            int scoreToAdd = isScoreBoosted ? 10 : 5; // Double score if ScoreBoost is active
            scoreManager.AddScorePlayerTwo(scoreToAdd); // Update score         
            Destroy(other.gameObject); // Destroy food
        }
        else if (other.CompareTag("MassBurner"))
        {
            if (bodySegments.Count > 0)
            {
                RemoveBodySegment(); // Removes a body segment if the snake has any
                scoreManager.AddScorePlayerTwo(-5); // Deduct 5 points
            }
            
            Destroy(other.gameObject); // Destroy food
        }

        else if (other.CompareTag("SpeedUp"))
        {
            Debug.Log("Speed!");
            ActivateSpeedBoost();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("ScoreBoost"))
        {
            Debug.Log("ScoreBoost!");
            ActivateScoreBoost();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Shield"))
        {
            Debug.Log("Shield!");
            ActivateShield();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Snake1Head") ||
                 (other.CompareTag("BodySegment") && other.transform.parent != transform))
        {
            Debug.Log("Snake-2 wins! Snake-1 has been hit.");
            EndGame("Snake-2");
        }
        else if (other.CompareTag("BodySegment") && other.transform.parent == transform && !isShieldActive)
        {
            Debug.Log("Snake-1 wins! Snake-2 bit its own body.");
            EndGame("Snake-1");
        }
    }

    private void EndGame(string winner)
    {
        Debug.Log($"{winner} wins the game!");
    }

    private void ActivateSpeedBoost()
    {
        if (!isSpeedBoosted)
        {
            isSpeedBoosted = true;
            moveInterval *= 0.5f;
            Invoke("DeactivateSpeedBoost", powerUpDuration);
        }
    }

    private void DeactivateSpeedBoost()
    {
        isSpeedBoosted = false;
        moveInterval = originalMoveInterval;
    }

    private void ActivateScoreBoost()
    {
        if (!isScoreBoosted)
        {
            isScoreBoosted = true;
            Invoke("DeactivateScoreBoost", powerUpDuration);
        }
    }

    private void DeactivateScoreBoost()
    {
        isScoreBoosted = false;
    }

    private void ActivateShield()
    {
        isShieldActive = true;
        Invoke("DeactivateShield", powerUpDuration);
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
    }



}













