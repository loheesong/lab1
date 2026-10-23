using UnityEngine;

public class FireFlowerPowerup : BasePowerup {
    void Awake() {
        powerupType = PowerupType.FireFlower;
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
            if (collectSound != null) AudioSource.PlayClipAtPoint(collectSound, transform.position);
            Destroy(gameObject);
        }
    }
}
