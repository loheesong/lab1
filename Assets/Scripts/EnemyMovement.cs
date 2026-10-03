using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    // ---------------------------- EVENTS & DELEGATES ----------------------------
    public delegate void EnemyStomped();
    public event EnemyStomped OnStomped;
    public event Action OnReset;

    // ---------------------------- MOVEMENT ----------------------------
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    private Collider2D enemyCollider;
    private Vector3 startPosition;
    // ---------------------------- STATE ----------------------------
    private bool isDead = false;
    public bool IsDead => isDead;

    // ---------------------------- VISUALS & ANIMATION ----------------------------
    [Header("Visuals & Animation")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite flatSprite;

    private SpriteRenderer spriteRenderer;

    // ---------------------------- AUDIO ----------------------------
    [Header("Audio")]
    [SerializeField] private AudioClip stompSound;
    private AudioSource audioSource;

    private void Awake() {
        startPosition = transform.position;
        enemyBody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        defaultSprite = spriteRenderer.sprite;
    }
    void Start() {
        ResetGoomba();
    }
    void FixedUpdate() {
        if (isDead) return;
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        CheckObstacle(); // Check for pipe/obstacle before moving!

        if (enemyBody.position.x > originalX + maxOffset) {
            moveRight = -1;
            ComputeVelocity();
        } else if (enemyBody.position.x < originalX - maxOffset) {
            moveRight = 1;
            ComputeVelocity();
        }
        Movegoomba();
    }

    public void ResetGoomba() {
        StopAllCoroutines();
        isDead = false;
        gameObject.SetActive(true);
        enemyCollider.enabled = true;
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = defaultSprite;
        transform.position = startPosition;
        originalX = transform.position.x;
        moveRight = -1; // start moving left
        ComputeVelocity();
        OnReset?.Invoke();
    }

    void ComputeVelocity() {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba() {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    public void Stomp() {
        if (isDead) return;
        isDead = true;

        velocity = Vector2.zero;
        enemyCollider.enabled = false;
        spriteRenderer.sprite = flatSprite;
        audioSource.PlayOneShot(stompSound);

        OnStomped?.Invoke();
        StartCoroutine(FlattenAndDisappearRoutine());
    }

    private IEnumerator FlattenAndDisappearRoutine() {
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.enabled = false;
        gameObject.SetActive(false);
    }

    private void CheckObstacle() {
        Vector2 checkPos = (Vector2)transform.position + new Vector2(moveRight * 0.55f, 0f);
        Vector2 checkSize = new Vector2(0.2f, 0.8f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(checkPos, checkSize, 0f);
        foreach (Collider2D hit in hits) {
            if (hit.gameObject == gameObject) continue;
            if (hit.CompareTag("Player")) continue;

            if (hit.CompareTag("Obstacles") || hit.gameObject.layer == 7) {
                if (moveRight == -1 && hit.bounds.center.x < transform.position.x) {
                    moveRight = 1;
                    ComputeVelocity();
                    break;
                } else if (moveRight == 1 && hit.bounds.center.x > transform.position.x) {
                    moveRight = -1;
                    ComputeVelocity();
                    break;
                }
            }
        }
    }
}