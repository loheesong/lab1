using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private IntVariable currentScoreSO;
    [SerializeField] private IntGameEvent onScoreChanged;
    private void Start() {
        SubscribeToEnemies();
    }
    private void OnDestroy() {
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
        currentScoreSO.ApplyChange(1);
        onScoreChanged?.Raise(currentScoreSO.Value);
    }

    // Called by SimpleGameEventListener listening to OnGameRestart
    public void ResetAllEnemies() {
        foreach (Transform child in enemiesParent) {
            if (child.TryGetComponent<EnemyMovement>(out var enemy)) enemy.ResetGoomba();
        }
    }
}