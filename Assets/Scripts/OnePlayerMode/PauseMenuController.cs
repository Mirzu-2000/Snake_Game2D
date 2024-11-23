using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel; // Assign the Pause Panel in the inspector
    public SoundManager soundManager;

    private bool isPaused = false;
    private bool isGameOver = false; //  flag to disable pause functionality

    void Update()
    {
        // Disable pause functionality if the game is over
        if (isGameOver)
            return;

        {
            // Toggle pause screen with the Q key
            if (Input.GetKeyDown(KeyCode.Space))
            {
                soundManager.PlayButtonClick();
                if (isPaused)
                    ResumeGame();
                else
                    PauseGame();
            }
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
        soundManager.PlayButtonClick();
        isPaused = false;
        pausePanel.SetActive(false); // Hide pause panel
        Time.timeScale = 1f; // Resume the game
    }
    public void DeactivatePausePanel()
    {
        // Ensure pause panel is hidden and game is not paused
        isPaused = false;
        isGameOver = true; // Disable further pause input
        pausePanel.SetActive(false);
    }

    public void RestartGame()
    {
        soundManager.PlayButtonClick();
        Time.timeScale = 1f; // Reset time scale before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    public void ReturnToMainMenu()
    {
        soundManager.PlayButtonClick();
        Time.timeScale = 1f; // Reset time scale before loading the menu
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with your main menu scene name
    }
}
