using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign Game Over Panel in inspector
    public GameObject pausePanel; // Assign the Pause Panel in the inspector
    public PauseMenuController pauseMenuController; // Reference the PauseMenuController script

    public TMPro.TextMeshProUGUI highScoreText; // Assign TextMeshPro for High Score display
    public SoundManager soundManager;

  
    public void TriggerGameOver()
    {
        Time.timeScale = 0f; // Pause the game
        gameOverPanel.SetActive(true); // Show Game Over Panel

        // Deactivate pause panel through PauseMenuController
        pauseMenuController.DeactivatePausePanel();

        // Retrieve and display the high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;

        soundManager.PlayGameOver();
       
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time scale before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
        soundManager.PlayButtonClick();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before loading the menu
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your main menu scene name
        soundManager.PlayButtonClick();

    }
}
