using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] private Transform enemiesParent;
    private void Start() {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
        SubscribeToEnemies();
    }
    private void OnDestroy() {
        GameManager.Instance.OnStateChanged -= HandleStateChanged;
        UnsubscribeFromEnemies();
    }

    private void HandleStateChanged(GameState state) {
        if (state == GameState.Playing) ResetAllEnemies();
    }
    private void SubscribeToEnemies() {
        foreach (Transform child in enemiesParent) {
            if (child.TryGetComponent<EnemyMovement>(out var enemy)) enemy.OnStomped += HandleEnemyStomped;
        }
    }
    private void UnsubscribeFromEnemies() {
        foreach (Transform child in enemiesParent) {
            if (child.TryGetComponent<EnemyMovement>(out var enemy)) enemy.OnStomped -= HandleEnemyStomped;
        }
    }
    private void HandleEnemyStomped() {
        GameManager.Instance.AddScore(1);
    }
    public void ResetAllEnemies() {
        foreach (Transform child in enemiesParent) {
            if (child.TryGetComponent<EnemyMovement>(out var enemy)) enemy.ResetGoomba();
        }
    }
}