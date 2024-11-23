using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // If using TextMeshPro for UI elements

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen; // Reference to the Game Over UI panel
    public TextMeshProUGUI winnerText; // Text element to display the winner
    public string mainMenuSceneName = "MainMenu"; // Name of the main menu scene
    public TwoPlayerScoreManager scoreManager; // Reference to the TwoPlayerScoreManager script
    public PauseMenuController pauseMenuController; // Reference the PauseMenuController script
    public SoundManager soundManager;


    private bool isGameOver = false; // Tracks if the game is already over

    private void Start()
    {
        // Ensure the game over screen is hidden at the start
        gameOverScreen.SetActive(false);

        // Check if the score manager is assigned
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager is not assigned to GameOverManager!");
            enabled = false; // Disable this script to avoid runtime errors
        }
    }

    private void Update()
    {
        // Avoid running this logic if the game is already over
        if (isGameOver) return;

        // Check if either player's score is less than 0
        if (scoreManager.GetPlayerOneScore() < 0)
        {
            TriggerGameOver("Snake-2");
        }
        else if (scoreManager.GetPlayerTwoScore() < 0)
        {
            TriggerGameOver("Snake-1");
        }
    }

    // Method to trigger game over
    public void TriggerGameOver(string winner)
    {
        if (isGameOver) return; // Prevent multiple calls to game over logic

        isGameOver = true; // Set the game over state

        // Deactivate pause panel through PauseMenuController
         pauseMenuController.DeactivatePausePanel();

        soundManager.PlayGameOver();

        // Display the game over screen and show the winner
        gameOverScreen.SetActive(true);
        winnerText.text = $"{winner} wins!"; // Display the winner

        // Pause the game
        Time.timeScale = 0f;
    }

    // Restart the current scene
    public void RestartGame()
    {
        soundManager.PlayButtonClick();
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    // Return to the main menu
    public void ReturnToMainMenu()
    {
        soundManager.PlayButtonClick();
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(mainMenuSceneName); // Load the main menu scene
    }
}
