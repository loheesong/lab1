using UnityEngine;

public class StarPowerup : BasePowerup {
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float speed = 2.5f;
    private Rigidbody2D rb;
    private int direction = 1;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        powerupType = PowerupType.StarMan;
    }

    void FixedUpdate() {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            ApplyPowerup(collision.gameObject);
            Destroy(gameObject);
            return;
        }

        // Bounce on floor
        if (collision.contacts[0].normal.y > 0.5f) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        } else if (Mathf.Abs(collision.contacts[0].normal.x) > 0.5f) {
            direction *= -1;
        }
    }

    public override void ApplyPowerup(GameObject target) {
        if (target.TryGetComponent<BuffStateController>(out var buff)) {
            buff.SetBuff(this.powerupType);
        }
    }
}