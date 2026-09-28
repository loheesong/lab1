using System.Collections;
using UnityEngine;

public class QuestionBoxController : MonoBehaviour {
    [Header("Visuals")]
    [SerializeField] private Sprite disabledSprite;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Coin Settings")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float coinBounceHeight = 1.8f;
    [SerializeField] private float coinBounceDuration = 0.55f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip coinSound;

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
        audioSource.PlayOneShot(coinSound);
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        StartCoroutine(CoinBounceRoutine(coin, spawnPos));

        // bounce box
        StartCoroutine(BounceRoutine());
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

    private IEnumerator CoinBounceRoutine(GameObject coin, Vector3 startPos) {
        float half = coinBounceDuration / 2f;
        float elapsed = 0f;
        Vector3 peakPos = startPos + Vector3.up * coinBounceHeight;
        // Coin arcs up
        while (elapsed < half) {
            if (coin == null) yield break;
            coin.transform.position = Vector3.Lerp(startPos, peakPos, elapsed / half);
            elapsed += Time.deltaTime;
            yield return null;
        }
        coin.transform.position = peakPos;
        // Coin falls back down
        elapsed = 0f;
        while (elapsed < half) {
            if (coin == null) yield break;
            coin.transform.position = Vector3.Lerp(peakPos, startPos, elapsed / half);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Destroy coin when it touches the box (looks like it went inside)
        Destroy(coin);
    }
}