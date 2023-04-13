using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScore : MonoBehaviour
{
    private Timer timer;
    private PlayerController player;
    [SerializeField] private float elapsedTime;
    [SerializeField] private float highScore;
    [SerializeField] private int levelIndex;
    [SerializeField] private TMP_Text highScoreText;


    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        player = FindObjectOfType<PlayerController>();
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        highScore = PlayerPrefs.GetFloat("highscore" + levelIndex);
    }

    private void Update()
    {
        // checks if game is not paused and that the player is not in a victory screen
        if (!timer.paused && !player.victory) elapsedTime += Time.deltaTime; // starts timer
        else
        {   // if the timer is paused and the player is in a victory screen
            // we then check if there is a highscore or none, if so, we set the new highscore time, we also use the levelIndex so we can track highscores for
            // multiple levels
            if (elapsedTime < highScore || highScore == 0) PlayerPrefs.SetFloat("highscore" + levelIndex, elapsedTime); 
        }
    }

    // this function converts the highscore into a string, so we can have a pretty string to display to the player
    private string ConvertHighScoreIntoString()
    {
        var minutes = Mathf.FloorToInt(highScore / 60);
        var seconds = Mathf.FloorToInt(highScore % 60);
        var currentTime = $"{minutes:00}:{seconds:00}";
        return currentTime;
    }

    // display highscore
    private void SetHighScoreText()
    {
        if (highScore != 0)
        {
            highScoreText.text = ConvertHighScoreIntoString();
        }
    }
    
}
