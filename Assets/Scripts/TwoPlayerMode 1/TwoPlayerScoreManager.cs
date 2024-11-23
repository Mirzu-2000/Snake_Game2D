using TMPro;
using UnityEngine;

public class TwoPlayerScoreManager : MonoBehaviour
{
    // Text UI references for both players
    public TextMeshProUGUI playerOneScoreText;
    public TextMeshProUGUI playerTwoScoreText;

    // Score values for each player
    private int playerOneScore = 0;
    private int playerTwoScore = 0;

    // Method to increase Player One's score
    public void AddScorePlayerOne(int points)
    {
        playerOneScore += points;
        UpdateScoreTextPlayerOne();
    }

    // Method to increase Player Two's score
    public void AddScorePlayerTwo(int points)
    {
        playerTwoScore += points;
        UpdateScoreTextPlayerTwo();
    }

    // Updates Player One's score display
    private void UpdateScoreTextPlayerOne()
    {
        playerOneScoreText.text = "Player-1 Score: " + playerOneScore;
    }

    // Updates Player Two's score display
    private void UpdateScoreTextPlayerTwo()
    {
        playerTwoScoreText.text = "Player-2 Score: " + playerTwoScore;
    }

    // Getter for Player One's score
    public int GetPlayerOneScore()
    {
        return playerOneScore;
    }

    // Getter for Player Two's score
    public int GetPlayerTwoScore()
    {
        return playerTwoScore;
    }

}
