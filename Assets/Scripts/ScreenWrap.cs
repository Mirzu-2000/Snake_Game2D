using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    // Screen boundary limits
    private float xBoundary = 18.0f;
    private float positiveYBoundary = 8.0f;
    private float negativeYBoundary = -10.0f;

    void Update()
    {
        Vector3 position = transform.position;

        // Check if the snake crosses the X boundaries
        if (position.x > xBoundary)
        {
            position.x = -xBoundary;
        }
        else if (position.x < -xBoundary)
        {
            position.x = xBoundary;
        }

        // Check if the snake crosses the Y boundaries
        if (position.y > positiveYBoundary)
        {
            position.y = negativeYBoundary; // Wraps from top to bottom
        }
        else if (position.y < negativeYBoundary)
        {
            position.y = positiveYBoundary; // Wraps from bottom to top
        }

        // Apply the new position if it has been changed
        transform.position = position;
    }
}
