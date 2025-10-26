using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1.0f;
    public float runMultiplier = 2.5f;
    public float jumpForce = 7.0f;

    [Header("Stamina Settings")]
    public float maxStamina = 5f;                  
    public float staminaDrainPerSecond = 1f;       
    public float staminaRechargePerSecond = 0.5f;  
    public float minStaminaToRun = 2f;             
    public float staminaRechargeDelay = 0.5f;        // delay sebelum recharge

    private float currentStamina;
    private float rechargeTimer = 0f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded = false;
    private bool isRunning = false;
    private float moveInput;

    private bool canRun = true;        
    private bool isForcedStopRun = false; 

    private enum Movement { Idle, Walk, Run };
    private Movement state = Movement.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        bool runKey = Input.GetKey(KeyCode.LeftShift);

        // --- Running & Stamina Logic ---
        if (runKey && moveInput != 0 && currentStamina >= minStaminaToRun && canRun && !isForcedStopRun)
        {
            isRunning = true;
            isForcedStopRun = false;

            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);

            // reset timer recharge karena player masih menahan shift
            rechargeTimer = 0f;

            if (currentStamina <= 0f)
            {
                isRunning = false;
                canRun = false;
                isForcedStopRun = true;
            }
        }
        else
        {
            isRunning = false;

            // Recharge hanya mulai setelah delay jika Shift dilepas
            if (!runKey)
            {
                rechargeTimer += Time.deltaTime;
                if (rechargeTimer >= staminaRechargeDelay)
                {
                    currentStamina += staminaRechargePerSecond * Time.deltaTime;
                    currentStamina = Mathf.Min(currentStamina, maxStamina);
                }
            }
            else
            {
                // jika masih menahan Shift, timer tetap 0
                rechargeTimer = 0f;
            }

            if (!runKey)
                canRun = true;
        }

        // --- Jump ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // --- Flip Player ---
        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);

        UpdateAnimationState();
    }

    void FixedUpdate()
    {
        float currentSpeed = isRunning ? moveSpeed * runMultiplier : moveSpeed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }

    void UpdateAnimationState()
    {
        if (moveInput != 0)
        {
            if (isRunning)
                state = Movement.Run;
            else if (isForcedStopRun)
                state = Movement.Walk;
            else
                state = Movement.Walk;
        }
        else
        {
            state = Movement.Idle;
        }

        animator.SetInteger("state", (int)state);
    }

    public float GetStaminaNormalized()
    {
        return currentStamina / maxStamina;
    }
}
