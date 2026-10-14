using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] private Transform enemiesParent;
    private void Start() {
        GameManager.Instance.OnGameRestart += ResetAllEnemies;
        SubscribeToEnemies();
    }
    private void OnDestroy() {
        GameManager.Instance.OnGameRestart -= ResetAllEnemies;
        UnsubscribeFromEnemies();
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