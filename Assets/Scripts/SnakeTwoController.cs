using UnityEngine;
using System.Collections.Generic;

public class SnakeTwoController : MonoBehaviour
{
    //public float moveSpeed = 0.1f;
    public float gridSize = 1.0f; // Size of one grid unit
    public float moveInterval = 0.3f; // Time in seconds between each move
    private float moveTimer = 0.0f;
    public GameObject bodySegmentPrefab; // Assign in the inspector
    private Vector2 moveDirection = Vector2.left;
    private List<Transform> bodySegments = new List<Transform>();
    private bool canChangeDirection = true;

    void Start()
    {
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

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision!");
        if (other.CompareTag("MassGainer"))
        {
            AddBodySegment(); // Adds a body segment to the snake
                              // score += 5;
            Destroy(other.gameObject); // Destroy food
        }
        else if (other.CompareTag("MassBurner"))
        {
            if (bodySegments.Count > 0)
            {
                RemoveBodySegment(); // Removes a body segment if the snake has any
            }
            // score -= 5;
            Destroy(other.gameObject); // Destroy food
        }

        // UpdateScoreDisplay();
    }












}
