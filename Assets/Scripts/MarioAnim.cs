using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class MarioAnim : MonoBehaviour {
    [Header("Subject Reference")]
    [SerializeField] private PlayerMovement playerMovement;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (playerMovement == null) {
            playerMovement = GetComponentInParent<PlayerMovement>();
        }
    }

    private void Start() {
        // Sync initial state on game start
        if (playerMovement != null) {
            animator.SetBool("onGround", playerMovement.IsGrounded);
        }
    }

    // SUBSCRIPTION LIFECYCLE
    private void OnEnable() {
        if (playerMovement == null) return;

        // Subscribe to Subject events
        playerMovement.OnSkid += HandleSkid;
        playerMovement.OnGroundedChanged += HandleGroundedChanged;
        playerMovement.OnDirectionChanged += HandleDirectionChanged;
        playerMovement.OnPlayerReset += HandlePlayerReset;
    }

    private void OnDisable() {
        if (playerMovement == null) return;

        // Unsubscribe to prevent memory leaks
        playerMovement.OnSkid -= HandleSkid;
        playerMovement.OnGroundedChanged -= HandleGroundedChanged;
        playerMovement.OnDirectionChanged -= HandleDirectionChanged;
        playerMovement.OnPlayerReset -= HandlePlayerReset;
    }

    // FRAME UPDATE (Continuous Parameters)
    private void Update() {
        if (playerMovement == null) return;

        // Continuously pass speed using the string name
        animator.SetFloat("xSpeed", playerMovement.CurrentSpeed);
    }

    // EVENT CALLBACKS (Discrete Actions)
    private void HandleSkid() {
        animator.SetTrigger("onSkid");
    }

    private void HandleGroundedChanged(bool isGrounded) {
        animator.SetBool("onGround", isGrounded);
    }

    private void HandleDirectionChanged(bool faceRight) {
        // Flip sprite: facing left means flipX is true
        spriteRenderer.flipX = !faceRight;
    }

    private void HandlePlayerReset() {
        animator.SetTrigger("gameRestart");
    }
}