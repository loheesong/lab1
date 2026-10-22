using System.Collections;
using UnityEngine;

public class BrickController : MonoBehaviour {
    [Header("Variant Settings")]
    [SerializeField] private bool hasCoin = false;
    [SerializeField] private int coinCount = 1; // Can be 1 for single-coin brick
    private int currCoinCount; // Can be 1 for single-coin brick

    [Header("Spawn & Prefabs")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip bumpSound; // smb_bump.wav
    private AudioSource audioSource;

    [Header("Bounce Settings")]
    [SerializeField] private float bounceHeight = 0.2f;
    [SerializeField] private float bounceDuration = 0.12f;

    private bool isBouncing = false;
    private Vector3 originalLocalPos;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
        originalLocalPos = transform.localPosition;

        currCoinCount = coinCount;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (isBouncing) return;

        if (IsHitFromBelow(collision)) {
            OnHit();
        }
    }

    private bool IsHitFromBelow(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Player")) return false;

        foreach (ContactPoint2D contact in collision.contacts) {
            if (contact.normal.y > 0.5f) return true;
        }
        return false;
    }

    private void OnHit() {
        // Variant 1: Contains a coin
        if (hasCoin && currCoinCount > 0) {
            currCoinCount--;

            Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        } else {
            // Variant 2: Standard brick (empty) -> play bump sound
            audioSource.PlayOneShot(bumpSound);
        }

        // Brick bounces once and does not break
        StartCoroutine(BounceRoutine());
    }

    public void ResetBrick() {
        currCoinCount = coinCount;
    }

    private IEnumerator BounceRoutine() {
        isBouncing = true;
        float half = bounceDuration / 2f;
        float elapsed = 0f;
        Vector3 peakPos = originalLocalPos + Vector3.up * bounceHeight;

        while (elapsed < half) {
            transform.localPosition = Vector3.Lerp(originalLocalPos, peakPos, elapsed / half);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = peakPos;

        elapsed = 0f;
        while (elapsed < half) {
            transform.localPosition = Vector3.Lerp(peakPos, originalLocalPos, elapsed / half);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalLocalPos;
        isBouncing = false;
    }
}