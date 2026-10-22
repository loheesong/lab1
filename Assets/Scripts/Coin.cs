using System.Collections;
using UnityEngine;

public class CoinController : MonoBehaviour {
    [Header("Bounce Settings")]
    [SerializeField] private float bounceHeight = 1.8f;
    [SerializeField] private float bounceDuration = 0.55f;

    [Header("Score & Events")]
    [SerializeField] private int points = 1;
    [SerializeField] private IntVariable currentScoreSO;
    [SerializeField] private IntGameEvent onScoreChanged;
    [SerializeField] private AudioClip coinSound;
    private AudioSource audioSource;
    private Vector3 startPos;

    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        startPos = transform.position;
        audioSource.PlayOneShot(coinSound);
        StartCoroutine(BounceRoutine());
    }

    private IEnumerator BounceRoutine() {
        currentScoreSO.ApplyChange(points);
        onScoreChanged?.Raise(currentScoreSO.Value);

        float half = bounceDuration / 2f;
        float elapsed = 0f;
        Vector3 peakPos = startPos + Vector3.up * bounceHeight;

        // Arc Upwards
        while (elapsed < half) {
            transform.position = Vector3.Lerp(startPos, peakPos, elapsed / half);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = peakPos;

        // Fall Downwards back to original position
        elapsed = 0f;
        while (elapsed < half) {
            transform.position = Vector3.Lerp(peakPos, startPos, elapsed / half);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
        Destroy(gameObject);
    }
}