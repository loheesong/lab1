using System;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup {
    [SerializeField] private float speed = 2f;
    private Rigidbody2D rb;
    private int direction = 1; // 1 = right, -1 = left

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        AudioManager.Instance.PlayPowerupAppearSound();
    }
    void FixedUpdate() {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }
    public override void ApplyPowerup() {
        Debug.Log("MUSHROOMMMMM");
        AudioManager.Instance.PlayPowerupSound();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            ApplyPowerup();
            Destroy(gameObject);
            return;
        }

        // Change direction if hitting a wall/obstacle on the side (ignore floor collisions)
        foreach (ContactPoint2D contact in collision.contacts) {
            if (Mathf.Abs(contact.normal.x) > 0.5f) {
                direction *= -1;
                break;
            }
        }
    }
}
