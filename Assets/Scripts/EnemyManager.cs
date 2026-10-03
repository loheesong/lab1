using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] private Transform enemiesParent;
    private void Start() {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }
    private void OnDestroy() {
        GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameState state) {
        if (state == GameState.Playing) ResetAllEnemies();
    }

    public void ResetAllEnemies() {
        foreach (Transform child in transform) {
            if (child.TryGetComponent<EnemyMovement>(out var enemy)) {
                enemy.ResetGoomba();
            }
        }
    }
}