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
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void Start() {
        // Sync initial state on game start
        animator.SetBool("onGround", playerMovement.IsGrounded);
    }

    // SUBSCRIPTION LIFECYCLE
    private void OnEnable() {
        if (playerMovement == null) return;
    }

    private void OnDisable() {
        if (playerMovement == null) return;
    }

    // FRAME UPDATE (Continuous Parameters)
    private void Update() {
        if (!playerMovement.alive) return;
        // Continuously pass speed using the string name
        animator.SetFloat("xSpeed", playerMovement.CurrentSpeed);
    }

    // EVENT CALLBACKS
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
    private void HandlePlayerDeath() {
        animator.SetBool("onGround", true);
        animator.SetFloat("xSpeed", 0f);
        animator.Play("mario-die");
    }
}