using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Timer timer;
    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool IsMoving;
    private bool canMove;
    private Vector2 pInput;
    [Header("Health")] 
    [SerializeField] private float currentHealth;
    private const float MaxHealth = 100f;
    [Header("UI")]
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject GameOverScreen;

    public bool victory;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        timer = FindObjectOfType<Timer>();
        currentHealth = MaxHealth;
        canMove = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = pInput * movementSpeed;
        switch (IsMoving)
        {
            case true:
                animator.SetFloat("X", pInput.x);
                animator.SetFloat("Y", pInput.y);
                break;
            case false:
                animator.SetFloat("X", 0f);
                animator.SetFloat("Y", 0f);
                break;
        }
        if (currentHealth == 0)
        {
            pInput = Vector2.zero;
            StartCoroutine(Respawn());
        }

        if (victory)
        {
            pInput = Vector2.zero;
            StartCoroutine(Win());
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (canMove)
        {
            pInput = context.ReadValue<Vector2>();
            IsMoving = pInput != Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Obstacle")) currentHealth = 0;
        if (col.gameObject.CompareTag("Victory")) victory = true;
    }
    public IEnumerator Respawn()
    {
        animator.SetTrigger("Death");
        timer.paused = true;
        StopPlayerInput();
        yield return new WaitForSeconds(2f);
        GameOverScreen.SetActive(true);
        
    }
    private IEnumerator Win()
    {
        animator.SetTrigger("Victory");
        timer.paused = true;
        StopPlayerInput();
        yield return new WaitForSeconds(2f);
        VictoryScreen.SetActive(true);
    }

    private void StopPlayerInput()
    {
        pInput = Vector2.zero;
        canMove = false;
    }
}
