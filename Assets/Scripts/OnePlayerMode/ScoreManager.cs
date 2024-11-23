using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the Text UI element
    private int score = 0; // Starting score

    // Method to increase the score
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Update the score display
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
