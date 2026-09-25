using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D rb;
    private BoxCollider2D col;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    // ---------------------------- MOVEMENT ----------------------------
    private Vector2 moveInput;

    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float acceleration = 90f;
    [SerializeField] private float deceleration = 70f;
    [SerializeField] private float airAcceleration = 60f;
    [SerializeField] private float airDeceleration = 30f;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private float timeToJumpApex = 0.35f;
    [SerializeField] private float downwardMovementMultiplier = 2.2f;
    [SerializeField] private float jumpCutMultiplier = 2.5f;
    [SerializeField] private float apexHangThreshold = 1.2f;
    [SerializeField] private float apexHangGravityMultiplier = 0.5f;
    private float gravityStrength;
    private float initialJumpVelocity;
    // states
    private bool isGrounded = true;

    [Header("Forgiveness")]
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    // ---------------------------- ANIMATION ----------------------------
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public event Action OnSkid;                     // on rapid direction change
    public event Action<bool> OnGroundedChanged;    // when landing or taking off
    public event Action<bool> OnDirectionChanged;   // when changing left/right
    public event Action OnPlayerReset;              // when ResetGame() is called
    public event Action OnPlayerDeath;              // on death (for next sections)
    public float CurrentSpeed => Mathf.Abs(rb.linearVelocity.x);
    public bool IsGrounded => isGrounded;

    [Header("Death Settings")]
    public float deathImpulse = 5f;
    [System.NonSerialized] public bool alive = true;

    // ---------------------------- UI ----------------------------
    public TextMeshProUGUI scoreText;
    [SerializeField] private GameObject uiScreen;

    // ---------------------------- ENEMIES ----------------------------
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0f;
        CalculateJumpVariables();
    }

    // Start is called before the first frame update
    void Start() {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (!alive) return;

        GatherInput();
        CheckCollisions();
        UpdateTimers();
        HandleJump();

        flipSprite();
    }

    // FixedUpdate is called 50 times a second
    void FixedUpdate() {
        if (!alive) {
            rb.linearVelocityY -= 20f * Time.fixedDeltaTime;
            return;
        }

        ApplyHorizontalMovement();
        ApplyCustomGravity();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy") && alive) {
            Debug.Log("Collided with goomba!");

            // process death 
            alive = false;
            rb.linearVelocity = Vector2.zero;
            OnPlayerDeath?.Invoke();
        }
    }

    // ---------------------------- MOVEMENT ----------------------------
    private void CheckCollisions() {
        Bounds bounds = col.bounds;
        // Make the check box slightly narrower than the player so walls aren't flagged as floors
        Vector2 checkSize = new Vector2(bounds.size.x - 0.04f, 0.08f);
        bool currentlyGrounded = Physics2D.OverlapBox(new Vector2(bounds.center.x, bounds.min.y), checkSize, 0f, groundLayer);

        if (currentlyGrounded != isGrounded) {
            isGrounded = currentlyGrounded;
            OnGroundedChanged?.Invoke(isGrounded);
        }
    }
    private void GatherInput() {
        // Using legacy raw axis polling for instantaneous response
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    // horizontal related
    private void ApplyHorizontalMovement() {
        float targetSpeed = moveInput.x * moveSpeed;

        // Choose acceleration or deceleration depending on current intent and state
        float accelRate;
        if (isGrounded) {
            accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        } else {
            accelRate = Mathf.Abs(targetSpeed) > 0.01f ? airAcceleration : airDeceleration;
        }

        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float movement = speedDiff * accelRate * Time.fixedDeltaTime;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x + movement, rb.linearVelocity.y);
    }

    // vertical related 
    private void HandleJump() {
        if (Input.GetButtonDown("Jump") && coyoteTimeCounter > 0f) {
            rb.linearVelocityY = initialJumpVelocity;
            coyoteTimeCounter = 0f;

            isGrounded = false;
            OnGroundedChanged?.Invoke(false);
        }
    }
    private void CalculateJumpVariables() {
        gravityStrength = 2f * jumpHeight / Mathf.Pow(timeToJumpApex, 2f);
        initialJumpVelocity = gravityStrength * timeToJumpApex;
    }
    private void ApplyCustomGravity() {
        float currentGravity = gravityStrength;

        // Condition 1: Falling -> Plummet briskly
        if (rb.linearVelocity.y < 0f) {
            currentGravity *= downwardMovementMultiplier;
        }
        // Condition 2: Early button release -> Cut jump short
        else if (rb.linearVelocity.y > 0f && !Input.GetButton("Jump")) {
            currentGravity *= jumpCutMultiplier;
        }
        // Condition 3: At the crest of the arc -> Float briefly
        else if (Mathf.Abs(rb.linearVelocity.y) < apexHangThreshold) {
            currentGravity *= apexHangGravityMultiplier;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y - (currentGravity * Time.fixedDeltaTime));
    }
    private void UpdateTimers() {
        coyoteTimeCounter = isGrounded ? coyoteTime : coyoteTimeCounter - Time.deltaTime;
    }

    private void OnDrawGizmosSelected() {
        if (col == null) return;
        Gizmos.color = Color.green;
        Bounds b = col.bounds;
        Gizmos.DrawWireCube(new Vector2(b.center.x, b.min.y), new Vector2(b.size.x - 0.04f, 0.08f));
    }
    // ---------------------------- ANIMATION ----------------------------
    private void flipSprite() {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState) {
            // skid
            if (isGrounded && rb.linearVelocity.x > 0.1f) {
                OnSkid?.Invoke();
            }
            faceRightState = false;
            OnDirectionChanged?.Invoke(faceRightState);
        }

        if (Input.GetKeyDown("d") && !faceRightState) {
            // skid
            if (isGrounded && rb.linearVelocity.x < -0.1f) {
                OnSkid?.Invoke();
            }
            faceRightState = true;
            OnDirectionChanged?.Invoke(faceRightState);
        }
    }
    // Called by an Animation Event at the start of mario-die
    public void PlayDeathImpulse() {
        rb.linearVelocity = new Vector2(0f, deathImpulse);
    }
    // Called by an Animation Event at the end of mario-die
    public void GameOverScene() {
        Time.timeScale = 0.0f; // Stop time only after the death animation finishes
        uiScreen.SetActive(true);
    }
    // ---------------------------- UI ----------------------------
    public void RestartButtonCallback(int input) {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame() {
        alive = true;
        rb.linearVelocity = Vector2.zero;

        rb.transform.position = new Vector3(-5.33f, -4.69f, 0.0f);
        faceRightState = true;
        OnDirectionChanged?.Invoke(faceRightState);

        // Notify observers to trigger gameRestart
        OnPlayerReset?.Invoke();

        // Reset scores & enemies 
        scoreText.text = "Score: 0";
        foreach (Transform eachChild in enemies.transform) {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
        jumpOverGoomba.score = 0;
        uiScreen.SetActive(false);
    }
}