using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private Timer timer;
    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isMoving;
    private bool canMove;
    private Vector2 pInput;
    [Header("UI")]
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject gameOverScreen;

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
    // checks for collision on the shrine
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Victory")) victory = true;
    }
    // respawns player if no time left
    public IEnumerator Respawn()
    {
        animator.SetTrigger(Death);
        timer.paused = true;
        StopPlayerInput();
        yield return new WaitForSeconds(2f);
        gameOverScreen.SetActive(true);
        
    }
    // plays wining dance and shows victory screen
    private IEnumerator Win()
    {
        animator.SetTrigger(Victory);
        timer.paused = true;
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
