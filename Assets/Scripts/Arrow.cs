using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    [Header("Components")] 
    private Timer timer;
    [Header("Arrow list")]
    [SerializeField] private Image arrowDown;
    [SerializeField] private Image arrowUp;
    [SerializeField] private Image arrowLeft;
    [SerializeField] private Image arrowRight;
    [SerializeField] private Image chosenArrow;
    private Image[] arrowArray;
    [Header("Timers")] 
    [SerializeField] private float timeToPress;
    [SerializeField] private float timeToSpawn;
    [SerializeField] private float elapsedTime;
    [SerializeField] private bool success;
    [SerializeField] private bool failed;
    [Header("Intensifier")] 
    [SerializeField] private Image intensifier;
    [Header("Time UI")] 
    [SerializeField] private Image plusTen;
    [SerializeField] private Image minusTen;

    
    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
        arrowArray = new[] { arrowDown, arrowUp, arrowLeft, arrowRight };
        // this is not needed, but when I was tweaking the values and checking things up I kept forgetting to set it to 0
        // so I just coded it so I wouldn't have to worry about it and everytime I pressed play no matter what I could test properly
        ChangeImageAlpha(arrowDown, 0f);
        ChangeImageAlpha(arrowUp, 0f);
        ChangeImageAlpha(arrowLeft, 0f);
        ChangeImageAlpha(arrowRight, 0f);
        ChangeImageAlpha(intensifier, 0f);
        ChangeImageAlpha(plusTen, 0f);
        ChangeImageAlpha(minusTen, 0f);
        intensifier.enabled = false;
    }
    
    private void Update()
    {
        if (!timer.paused) StartCoroutine(ShowArrowOnHUD());
    }
    
    // randomizes an arrow to be chosen for the player to press
    private Image PickRandomArrow()
    {
        var randomImage = Random.Range(0, arrowArray.Length);
        return randomImage switch
        {
            0 => arrowDown,
            1 => arrowUp,
            2 => arrowLeft,
            3 => arrowRight,
            _ => null,
        };
    }
    
    // shows the arrow to the player
    private IEnumerator ShowArrowOnHUD()
    {
        elapsedTime += Time.deltaTime;
        if (!(elapsedTime > timeToSpawn)) yield break;
        elapsedTime = 0;
        failed = false;
        success = false;
        chosenArrow = PickRandomArrow();
        intensifier.enabled = true;
        ChangeImageAlpha(chosenArrow, 255f);
        yield return new WaitForSeconds(timeToPress);
        if (!success)
        {
            timer.UpdateTimer(-10f);
            ChangeImageAlpha(chosenArrow, 0f);
            intensifier.enabled = false;
            elapsedTime = 0;
            failed = true;
            StartCoroutine(ShowPenaltyOrBonusUI(minusTen));
        }
    }
    // checks if the player pressed the correct input, making sure that the player only has one attempt
    // and if the timer runs out that attempt is gone
    private void CheckPlayerInput(Image arrow, string arrowString)
    {
        if (arrow.gameObject.name != arrowString) failed = true;
        if (!failed)
        {
            if (arrow.gameObject.name == arrowString && !success)
            {
                ChangeImageAlpha(chosenArrow, 0f);
                intensifier.enabled = false;
                elapsedTime = 0;
                success = true;
                failed = false;
                StartCoroutine(ShowPenaltyOrBonusUI(plusTen));
                timer.UpdateTimer(+10f);
            }
        }
    }
    
    private IEnumerator ShowPenaltyOrBonusUI(Image image)
    {
        ChangeImageAlpha(image, 255f);
        yield return new WaitForSeconds(1.5f);
        ChangeImageAlpha(image, 0f);
    }
    // little helper to change an images alpha so I don't have to keep repeating myself
    private void ChangeImageAlpha(Image image, float value)
    {
        var temp = image.color;
        temp.a = value;
        image.color = temp;
    }
    
    // manages players input for each individual arrow
    
    public void OnArrowUp(InputAction.CallbackContext context)
    { 
        if (context.performed && chosenArrow != null) CheckPlayerInput(chosenArrow, "arrow-up");
    }
    
    public void OnArrowDown(InputAction.CallbackContext context)
    { 
        if (context.performed && chosenArrow != null) CheckPlayerInput(chosenArrow, "arrow-down");
    }
    
    public void OnArrowLeft(InputAction.CallbackContext context)
    {
        if (context.performed && chosenArrow != null) CheckPlayerInput(chosenArrow, "arrow-left");
    }
    
    public void OnArrowRight(InputAction.CallbackContext context)
    {
        if (context.performed && chosenArrow != null) CheckPlayerInput(chosenArrow, "arrow-right");
    }
}