using System.Collections; // Wajib ditambahkan untuk menggunakan Coroutine
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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
    public float staminaRechargeDelay = 0.5f;

    [Header("Audio Settings")]
    public AudioClip footstepSound;
    // --- BARIS BARU: Variabel untuk jeda langkah kaki ---
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.3f;
    // ----------------------------------------------------

    private float currentStamina;
    private float rechargeTimer = 0f;
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private bool isGrounded = false;
    private bool isRunning = false;
    private float moveInput;
    private bool canRun = true;
    private bool isForcedStopRun = false;
    private enum Movement { Idle, Walk, Run };
    private Movement state = Movement.Idle;

    // --- BARIS BARU: Variabel untuk mengontrol Coroutine ---
    private Coroutine footstepCoroutine;
    // ------------------------------------------------------

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;
        audioSource = GetComponent<AudioSource>();
        
        // --- PENTING: Loop sekarang di-nonaktifkan ---
        audioSource.loop = false;
        // --------------------------------------------
        audioSource.clip = footstepSound;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        bool runKey = Input.GetKey(KeyCode.LeftShift);

        // (Logika Stamina & Gerakan tidak ada perubahan)
        #region Stamina and Movement Logic
        if (runKey && moveInput != 0 && currentStamina >= minStaminaToRun && canRun && !isForcedStopRun)
        {
            isRunning = true;
            isForcedStopRun = false;
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);
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
                rechargeTimer = 0f;
            }
            if (!runKey)
                canRun = true;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        #endregion

        UpdateAnimationState();
        
        // --- LOGIKA AUDIO SEKARANG MENGATUR COROUTINE ---
        HandleFootstepAudio();
    }
    
   
    void HandleFootstepAudio()
    {
        // Jika player bergerak di darat
        if (moveInput != 0 && isGrounded)
        {
            // Dan jika coroutine belum berjalan, maka mulai
            if (footstepCoroutine == null)
            {
                footstepCoroutine = StartCoroutine(FootstepCoroutine());
            }
        }
        // Jika player berhenti atau melompat
        else
        {
            // Dan jika coroutine sedang berjalan, maka hentikan
            if (footstepCoroutine != null)
            {
                StopCoroutine(footstepCoroutine);
                footstepCoroutine = null;
                
              
                // Hentikan suara yang sedang diputar secara paksa
                audioSource.Stop();
              
            }
        }
    }

    // --- COROUTINE BARU: Ini yang mengatur jeda suara ---
    IEnumerator FootstepCoroutine()
    {
        while (true) // Looping selamanya (akan dihentikan oleh StopCoroutine)
        {
            // Tentukan interval berdasarkan state jalan atau lari
            float interval = isRunning ? runStepInterval : walkStepInterval;
            
            // Putar suara sekali
            audioSource.Play();
            
            // Tunggu selama interval sebelum mengulang loop
            yield return new WaitForSeconds(interval);
        }
    }
    
    // Sisa kode lainnya tetap sama...
    #region Unchanged Code
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
            state = isRunning ? Movement.Run : Movement.Walk;
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
    #endregion
}