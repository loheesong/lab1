using System;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D rb;
    private BoxCollider2D col;

    [Header("Camera")]
    public Transform gameCamera;
    [SerializeField] private Transform cameraSpawnPoint;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform spawnPoint;
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
    private bool canDoubleJump;

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
    public event Action OnPlayerDeath;              // on death
    public float CurrentSpeed => Mathf.Abs(rb.linearVelocity.x);
    public bool IsGrounded => isGrounded;

    [Header("Death Settings")]
    public float deathImpulse = 5f;
    [System.NonSerialized] public bool alive = true;

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

        // Set Mario to spawn point
        transform.position = spawnPoint.position;
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
        if (Input.GetButtonDown("Jump")) {
            // First jump (grounded or within coyote time)
            if (coyoteTimeCounter > 0f) {
                rb.linearVelocityY = initialJumpVelocity;
                coyoteTimeCounter = 0f;
                isGrounded = false;
                OnGroundedChanged?.Invoke(false);
            }
            // Mid-air double jump
            else if (canDoubleJump) {
                rb.linearVelocityY = initialJumpVelocity;
                canDoubleJump = false; // Consume the double jump
            }
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
        if (isGrounded) {
            coyoteTimeCounter = coyoteTime;
            canDoubleJump = true; // Refill double jump on ground
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }
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
    public void PlayDeathImpulse() {
        rb.linearVelocity = new Vector2(0f, deathImpulse);
    }
    public void GameOverScene() {
        GameManager.Instance.SetGameState(GameState.GameOver);
    }
    // ---------------------------- UI ----------------------------
    public void ResetPlayer() {
        alive = true;
        canDoubleJump = true;
        rb.linearVelocity = Vector2.zero;
        transform.position = spawnPoint.position;
        faceRightState = true;
        OnDirectionChanged?.Invoke(faceRightState);
        gameCamera.position = cameraSpawnPoint.position;
        // Notify observers (e.g. animation reset)
        OnPlayerReset?.Invoke();
    }
}