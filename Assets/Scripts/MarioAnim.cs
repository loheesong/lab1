using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class MarioAnim : MonoBehaviour {
    [Header("Subject Reference")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private BoolVariable marioFaceRightSO;

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

    // FRAME UPDATE (Continuous Parameters)
    private void Update() {
        if (!playerMovement.alive) return;

        animator.SetBool("onGround", playerMovement.IsGrounded);
        animator.SetFloat("xSpeed", playerMovement.CurrentSpeed);
        spriteRenderer.flipX = !marioFaceRightSO.Value;
    }
}