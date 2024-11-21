using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameOverController gameOverController; // Reference to the GameOverController

    private int score = 0; // Current score
    private int highScore = 0; // High score

    void Start()
    {
        // Load the high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreText();
        UpdateHighScoreText();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetHighScore();

        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();

        // Trigger game over if score drops below 0
        if (score < 0)
        {
            GameOver();
        }
        else
        {
            UpdateHighScore(); // Check and update high score if needed
        }

    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore;
    }

    private void UpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            UpdateHighScoreText();
            SaveHighScore(); // Save the new high score
        }
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore); // Save the high score
        PlayerPrefs.Save(); // Ensure data is written to disk
    }

    private void ResetHighScore()
    {
        highScore = 0;
        PlayerPrefs.DeleteKey("HighScore"); // Delete the high score from PlayerPrefs
        UpdateHighScoreText(); // Update the UI
    }

    private void GameOver()
    {
        if (gameOverController != null)
        {
            gameOverController.TriggerGameOver(); // Trigger game over
        }
        else
        {
            Debug.LogWarning("GameOverController is not assigned in the ScoreManager.");
        }
    }

}
