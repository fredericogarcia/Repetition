using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        if (!timer.paused && !player.victory) elapsedTime += Time.deltaTime;
        else
        {
            if (elapsedTime < highScore || highScore == 0) PlayerPrefs.SetFloat("highscore" + levelIndex, elapsedTime);
        }
    }

    private string ConvertHighScoreIntoString()
    {
        var minutes = Mathf.FloorToInt(highScore / 60);
        var seconds = Mathf.FloorToInt(highScore % 60);
        var currentTime = $"{minutes:00}:{seconds:00}";
        return currentTime;
    }

    private void SetHighScoreText()
    {
        if (highScore != 0)
        {
            highScoreText.text = ConvertHighScoreIntoString();
        }
    }
    
}
