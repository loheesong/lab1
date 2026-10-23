using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class FireballController : MonoBehaviour {
    [SerializeField] private float lifetime = 3.0f;
    [SerializeField] private float bounceVelocity = 5.0f;
    [SerializeField] private AudioClip fireballSound;
    private AudioSource audioSource;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
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
        if (collision.gameObject.CompareTag("Player")) return;

        if (collision.gameObject.CompareTag("Enemy")) {
            if (collision.gameObject.TryGetComponent<EnemyMovement>(out var enemy)) {
                enemy.Stomp();
            }
            Destroy(gameObject);
            return;
        }

        // 2. Bounce on ground
        foreach (ContactPoint2D contact in collision.contacts) {
            if (contact.normal.y > 0.5f) {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceVelocity);
                return;
            } else if (Mathf.Abs(contact.normal.x) > 0.5f) {
                // Hit wall
                Destroy(gameObject);
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {            // add whole method
        if (other.CompareTag("Enemy") && other.TryGetComponent<EnemyMovement>(out var enemy)) {
            enemy.Stomp();
            Destroy(gameObject);
        }
    }

    public void Despawn() => Destroy(gameObject);
}