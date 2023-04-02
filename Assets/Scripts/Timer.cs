using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Components")] 
    private PlayerController playerController;
    [Header("Timer")]
    [SerializeField] private TMP_Text timeDisplay;
    [SerializeField] private Image timerDisplayBackground;
    [SerializeField] private float timeToComplete;
    [SerializeField] private Sprite displayBackgroundWhite;
    [SerializeField] private Sprite displayBackgroundRed;
    private string currentTime;
    public bool paused;
    

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        switch (timeToComplete)
        {
            case > 0 when !paused:
                timeToComplete -= Time.deltaTime;
                UpdateTimerDisplay();
                break;
            case <= 0f:
                timeDisplay.text = "00:00";
                StartCoroutine(playerController.Respawn());
                break;
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timeToComplete > 30f) timerDisplayBackground.sprite = displayBackgroundWhite;
        else if (timeToComplete <= 30f) timerDisplayBackground.sprite = displayBackgroundRed;

        var minutes = Mathf.FloorToInt(timeToComplete / 60);
        var seconds = Mathf.FloorToInt(timeToComplete % 60);

        currentTime = $"{minutes:00}:{seconds:00}";
        timeDisplay.text = currentTime;

    }

    public void UpdateTimer(float value)
    {
        timeToComplete += value;
    }
}
