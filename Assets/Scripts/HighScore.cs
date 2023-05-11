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
    [SerializeField] private TMP_Text elapsedTimeText;


    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        player = FindObjectOfType<PlayerController>();
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        highScore = PlayerPrefs.GetFloat("highscore" + levelIndex);
    }

    private void Update()
    {
        SetElapsedTimeText();
        SetHighScoreText(highScore);
        // checks if game is not paused and that the player is not in a victory screen
        if (!timer.paused && !player.victory) elapsedTime += Time.deltaTime; // starts timer
        else if (elapsedTime < highScore || highScore == 0)
        {   // if the timer is paused and the player is in a victory screen
            // we then check if there is a highscore or none, if so, we set the new highscore time, we also use the levelIndex so we can track highscore for
            // multiple levels
            if (player.victory) {
                PlayerPrefs.SetFloat("highscore" + levelIndex, elapsedTime);
                SetHighScoreText(PlayerPrefs.GetFloat("highscore" + levelIndex, elapsedTime));
            } 
        }
    }

    // this function converts the highscore into a string, so we can have a pretty string to display to the player
    private static string ConvertTimeIntoString(float time)
    {
        var minutes = Mathf.FloorToInt(time / 60);
        var seconds = Mathf.FloorToInt(time % 60);
        var currentTime = $"{minutes:00}:{seconds:00}";
        return currentTime;
    }

    // display highscore
    private void SetHighScoreText(float score) => highScoreText.text = ConvertTimeIntoString(score);
    private void SetElapsedTimeText() =>  elapsedTimeText.text = ConvertTimeIntoString(elapsedTime);
    
}
