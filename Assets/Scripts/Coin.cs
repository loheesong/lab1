using System.Collections;
using UnityEngine;

public class CoinEffect : MonoBehaviour {
    [Header("Audio")]
    [SerializeField] private AudioClip coinSound;
    private AudioSource audioSource;

    [Header("Motion")]
    [SerializeField] private float bounceHeight = 2.0f;
    [SerializeField] private float duration = 0.5f;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        // Play the coin sound
        audioSource.PlayOneShot(coinSound);
        StartCoroutine(PopUpAndReturn());
    }

    private IEnumerator PopUpAndReturn() {
        Vector3 startPos = transform.position;
        Vector3 peakPos = startPos + Vector3.up * bounceHeight;

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        // Move upward to peak
        while (elapsed < halfDuration) {
            transform.position = Vector3.Lerp(startPos, peakPos, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = peakPos;

        // Fall back down into the box
        elapsed = 0f;
        while (elapsed < halfDuration) {
            transform.position = Vector3.Lerp(peakPos, startPos, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Once returned to the box, destroy the coin
        Destroy(gameObject);
    }
}