using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Methods to handle button clicks

    // Load the One Player Mode scene
    public void LoadOnePlayerMode()
    {
        SceneManager.LoadScene("OnePlayerMode"); // Replace with your exact One Player Mode scene name
    }

    // Load the Two Player Mode scene
    public void LoadTwoPlayerMode()
    {
        SceneManager.LoadScene("TwoPlayerMode"); // Replace with your exact Two Player Mode scene name
    }

    // Quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Game"); // Logs a message in the editor for testing
        Application.OpenURL("https://www.linkedin.com/in/mirza-ali-63921b191/"); // Redirects to a website    
    }
}