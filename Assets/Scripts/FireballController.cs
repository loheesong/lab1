using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class FireballController : MonoBehaviour {
    [SerializeField] private float lifetime = 2.0f;
    [SerializeField] private AudioClip fireballSound;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start() {
        audioSource.PlayOneShot(fireballSound);
        StartCoroutine(LifetimeRoutine());
    }

    private IEnumerator LifetimeRoutine() {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            if (collision.gameObject.TryGetComponent<EnemyMovement>(out var enemy)) {
                enemy.Stomp();
            }
            Destroy(gameObject);
        }
    }
}