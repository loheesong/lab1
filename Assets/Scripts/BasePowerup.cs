using UnityEngine;

public abstract class BasePowerup : MonoBehaviour, IPowerup {
    public PowerupType powerupType;
    protected bool consumed = false;

    public virtual void SpawnPowerup() { }
    public abstract void ApplyPowerup(GameObject target);

    public virtual void DestroyPowerup() {
        Destroy(gameObject);
    }
}