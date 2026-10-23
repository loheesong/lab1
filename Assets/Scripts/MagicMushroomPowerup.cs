using UnityEngine;

public class MagicMushroomPowerup : BasePowerup {
    [SerializeField] private float speed = 2f;
    private Rigidbody2D rb;
    private int direction = 1;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        powerupType = PowerupType.MagicMushroom;
    }

    void FixedUpdate() {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    public override void ApplyPowerup(GameObject target) {
        if (target.TryGetComponent<MarioStateController>(out var mario)) {
            mario.SetPowerup(this.powerupType);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            consumed = true;
            ApplyPowerup(collision.gameObject);
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            Destroy(gameObject);
            return;
        }

        foreach (ContactPoint2D contact in collision.contacts) {
            if (Mathf.Abs(contact.normal.x) > 0.5f) {
                direction *= -1;
                break;
            }
        }
    }
}