using UnityEngine;

public abstract class BasePowerup : MonoBehaviour, IPowerup {
    public PowerupType powerupType;
    protected bool consumed = false;

    public virtual void SpawnPowerup() {
        // Override for custom spawn logic
    }

    public abstract void ApplyPowerup();

    public virtual void DestroyPowerup() {
        Destroy(gameObject);
    }
}
