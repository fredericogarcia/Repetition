using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private Timer timer;
    private EventSystem eSystem;
    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isMoving;
    private bool canMove;
    private Vector2 pInput;
    [Header("UI")]
    [SerializeField] private GameObject playerHUD;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseScreenButton;
    [SerializeField] private GameObject victoryScreenButton;
    [SerializeField] private GameObject gameOverScreenButton;
    private bool isGamePaused;
    
    public bool victory;
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly int Victory = Animator.StringToHash("Victory");
    private static readonly int Death = Animator.StringToHash("Death");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = FindObjectOfType<Timer>();
        eSystem = EventSystem.current;
        canMove = true;
    }

    private void FixedUpdate()
    {
        // manages players input setting the correct values on the blend tree so the animations will play correctly
        rb.velocity = pInput * movementSpeed;
        switch (isMoving)
        {
            case true:
                animator.SetFloat(X, pInput.x);
                animator.SetFloat(Y, pInput.y);
                break;
            case false:
                animator.SetFloat(X, 0f);
                animator.SetFloat(Y, 0f);
                break;
        }
        if (victory) // if the player has won show victory screen and stops any possible input
        {
            pInput = Vector2.zero;
            StartCoroutine(Win());
        }
    }
    
    // player input
    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            pInput = context.ReadValue<Vector2>();
            isMoving = pInput != Vector2.zero;
        }
    }
    
    public void OnPause(InputAction.CallbackContext context)
    {
        // If the game is not already paused, pause the game by disabling the player HUD and showing the pause screen.
        if (!isGamePaused)
        {
            eSystem.SetSelectedGameObject(null);
            eSystem.SetSelectedGameObject(pauseScreenButton);
            isGamePaused = true;
            playerHUD.SetActive(!isGamePaused);
            pauseScreen.SetActive(isGamePaused);
            Time.timeScale = 0f;
            canMove = false;
        }
        // If the game is already paused, continue the game by enabling the player HUD and hiding the pause screen.
        else Continue();
    }

    public void Continue()
    {
        // This function continues the game by enabling the player HUD and hiding the pause screen.
        if (isGamePaused)
        {
            isGamePaused = false;
            playerHUD.SetActive(!isGamePaused);
            pauseScreen.SetActive(isGamePaused);
            Time.timeScale = 1f;
            canMove = true;
        }
    }
    
    // checks for collision on the shrine
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Victory")) victory = true;
    }
    // respawns player if no time left
    public IEnumerator Respawn()
    {
        timer.paused = true;
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(gameOverScreenButton);
        animator.SetTrigger(Death);
        StopPlayerInput();
        yield return new WaitForSeconds(2f);
        gameOverScreen.SetActive(true);
        
    }
    // plays wining dance and shows victory screen
    private IEnumerator Win()
    {
        timer.paused = true;
        eSystem.SetSelectedGameObject(null);
        eSystem.SetSelectedGameObject(victoryScreenButton);
        animator.SetTrigger(Victory);
        StopPlayerInput();
        yield return new WaitForSeconds(2f);
        victoryScreen.SetActive(true);
    }
    // used in the traps script so I could update the players speed
    public void UpdateMovementSpeed(float value)
    {
        movementSpeed += value;
    }
    // used in the traps script so I could save the current players speed
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }
    // disables all players input
    private void StopPlayerInput()
    {
        pInput = Vector2.zero;
        canMove = false;
    }
}
