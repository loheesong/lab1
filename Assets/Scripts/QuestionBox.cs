using System.Collections;
using UnityEngine;

public class QuestionBoxController : MonoBehaviour {
    [Header("Visuals")]
    [SerializeField] private Sprite disabledSprite;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Coin Settings")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Bounce Settings")]
    [SerializeField] private float bounceHeight = 0.25f;
    [SerializeField] private float bounceDuration = 0.12f;

    private bool isHit = false;
    private Vector3 originalLocalPos;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalLocalPos = transform.localPosition;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // if already disabled, do nothing (no bounce, no sound, no spawn)
        if (isHit) return;

        if (IsHitFromBelow(collision)) {
            TriggerQuestionBox();
        }
    }

    private bool IsHitFromBelow(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Player")) return false;
        foreach (ContactPoint2D contact in collision.contacts) {
            if (contact.normal.y > 0.5f) return true;
        }
        return false;
    }

    private void TriggerQuestionBox() {
        isHit = true;

        animator.enabled = false;
        spriteRenderer.sprite = disabledSprite;

        // spawn the animated coin
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);

        // bounce box
        StartCoroutine(BounceRoutine());
    }

    public void ResetQuestionBox() {
        isHit = false;
        animator.enabled = true;
    }

    private IEnumerator BounceRoutine() {
        float half = bounceDuration / 2f;
        float elapsed = 0f;

        Vector3 peakPos = originalLocalPos + Vector3.up * bounceHeight;

        // Bounce Up
        while (elapsed < half) {
            transform.localPosition = Vector3.Lerp(originalLocalPos, peakPos, elapsed / half);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = peakPos;

        // Return Down
        elapsed = 0f;
        while (elapsed < half) {
            transform.localPosition = Vector3.Lerp(peakPos, originalLocalPos, elapsed / half);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalLocalPos;
    }
}