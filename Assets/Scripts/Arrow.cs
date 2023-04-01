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
    
    private void ChangeImageAlpha(Image image, float value)
    {
        var temp = image.color;
        temp.a = value;
        image.color = temp;
    }
    
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