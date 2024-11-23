using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections;

public class SnakeController : MonoBehaviour
{
    
    public float gridSize = 1.0f; // Size of one grid unit
    public float moveInterval = 0.3f; // Time in seconds between each move
    private float moveTimer = 0.0f;
    public GameObject bodySegmentPrefab; // Assign in the inspector
    private List<Transform> bodySegments = new List<Transform>();
    private Vector2 moveDirection = Vector2.right;
    private bool canChangeDirection = true;
    public ScoreManager scoreManager; // Reference to ScoreManager script



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

        // Create the initial body segment as the tail
        AddBodySegment();
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
            if (Input.GetKeyDown(KeyCode.UpArrow) && moveDirection != Vector2.down) moveDirection = Vector2.up;
            else if (Input.GetKeyDown(KeyCode.DownArrow) && moveDirection != Vector2.up) moveDirection = Vector2.down;
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && moveDirection != Vector2.right) moveDirection = Vector2.left;
            else if (Input.GetKeyDown(KeyCode.RightArrow) && moveDirection != Vector2.left) moveDirection = Vector2.right;

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

        // Check if the snake collides with its body (self-collision)
        if (!isShieldActive && CheckSelfCollision())
        {
            Die();  // Call the death method if there's a self-collision
        }
    }

    private bool CheckSelfCollision()
    {
        // Check if the snake's head collides with any of its body segments
        for (int i = 0; i < bodySegments.Count; i++)
        {
            if (transform.position == bodySegments[i].position)
            {
                return true;  // Self-collision detected
            }
        }
        return false;  // No self-collision
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

            // Destroy the segment GameObject 
            Destroy(segmentToRemove.gameObject);
        }
    }

    private void Die()
    {
        // Stop the game or perform actions when the snake dies
        Debug.Log("Game Over! Snake collided with its body.");
        // Can call methods to stop the game, restart, or go to the game over screen here
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision!");
        if (other.CompareTag("MassGainer"))
        {
            AddBodySegment(); // Add a new body segment to the snake
            int scoreToAdd = isScoreBoosted ? 10 : 5; // Double score if ScoreBoost is active
            scoreManager.AddScore(scoreToAdd); // Update score
            Destroy(other.gameObject); // Remove the food object
        }
        else if (other.CompareTag("MassBurner"))
        {
            if (bodySegments.Count > 0)
            {
                RemoveBodySegment(); // Removes a body segment if the snake has any
                scoreManager.AddScore(-5); // Deduct 5 points
            }
            
            Destroy(other.gameObject); // Destroy food
        }

        // Check if the snake collided with a SpeedUp power-up
        else if (other.CompareTag("SpeedUp"))
        {
            ActivateSpeedBoost(); // Apply speed boost effect
            Destroy(other.gameObject); // Remove the power-up
        }
        // Check if the snake collided with a ScoreBoost power-up
        else if (other.CompareTag("ScoreBoost"))
        {
            ActivateScoreBoost(); // Apply score boost effect
            Destroy(other.gameObject); // Remove the power-up
        }

        // Check if the snake collided with a Shield power-up
        else if (other.CompareTag("Shield"))
        {
            ActivateShield(); // Activate the Shield power-up
            Destroy(other.gameObject); // Remove the power-up
        }



    }


    private void ActivateSpeedBoost()
    {
        // Only apply speed boost if not already active
        if (!isSpeedBoosted)
        {
            isSpeedBoosted = true; // Set flag to indicate speed boost is active
            moveInterval *= 0.5f; // Double the speed by halving the move interval
            Invoke("DeactivateSpeedBoost", powerUpDuration); // Schedule to deactivate after powerUpDuration
        }
    }

    private void DeactivateSpeedBoost()
    {
        // Reset to original speed after speed boost duration
        isSpeedBoosted = false;
        moveInterval = originalMoveInterval;
    }

    private void ActivateScoreBoost()
    {
        // Only apply score boost if not already active
        if (!isScoreBoosted)
        {
            isScoreBoosted = true; // Set flag to indicate score boost is active
            Invoke("DeactivateScoreBoost", powerUpDuration); // Schedule to deactivate after powerUpDuration
        }
    }

    private void DeactivateScoreBoost()
    {
        // Reset score multiplier after score boost duration
        isScoreBoosted = false;
    }


    // Activate the Shield power-up
    private void ActivateShield()
    {
        isShieldActive = true;  // Set the shield to active
        Invoke("DeactivateShield", powerUpDuration); // Deactivate shield after a certain duration
    }

    // Deactivate the Shield power-up after a set duration
    private void DeactivateShield()
    {
        isShieldActive = false; // Set the shield to inactive
    }


}
