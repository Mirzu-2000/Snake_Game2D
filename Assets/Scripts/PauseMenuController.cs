using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel; // Assign the Pause Panel in the inspector

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause screen with the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true); // Show pause panel
        Time.timeScale = 0f; // Pause the game
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); // Hide pause panel
        Time.timeScale = 1f; // Resume the game
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
