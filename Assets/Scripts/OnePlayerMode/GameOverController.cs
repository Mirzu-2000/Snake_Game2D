using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign Game Over Panel in inspector
    public TMPro.TextMeshProUGUI highScoreText; // Assign TextMeshPro for High Score display

    public void TriggerGameOver()
    {
        Time.timeScale = 0f; // Pause the game
        gameOverPanel.SetActive(true); // Show Game Over Panel

        // Retrieve and display the high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time scale before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before loading the menu
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your main menu scene name
    }
}
