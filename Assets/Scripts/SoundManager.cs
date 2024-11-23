using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource buttonClickSource;
    public AudioSource gameOverSource;

    public static SoundManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClick()
    {
        buttonClickSource.Play();
    }

    public void PlayGameOver()
    {
        gameOverSource.Play();
    }
}
