using System.Collections;
using UnityEngine;

public abstract class BasePowerup : MonoBehaviour, IPowerup {
    public PowerupType powerupType;
    [SerializeField] protected float lifetime = 10f;
    [SerializeField] protected AudioClip collectSound;

    protected bool consumed = false;

    protected virtual void Start() {
        if (lifetime > 0f) {
            StartCoroutine(ExpireRoutine());
        }
    }

    private IEnumerator ExpireRoutine() {
        yield return new WaitForSeconds(lifetime);
        if (!consumed) DestroyPowerup();
    }

    public virtual void SpawnPowerup() { }
    public abstract void ApplyPowerup(GameObject target);

    public virtual void DestroyPowerup() {
        Destroy(gameObject);
    }
}