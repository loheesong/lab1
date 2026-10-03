using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI inGameScoreText;
    [SerializeField] private TextMeshProUGUI endGameScoreText;
    [SerializeField] private GameObject gameOverUI;

    private void Start() {
        GameManager.Instance.OnScoreChanged += HandleScoreChanged;
        GameManager.Instance.OnStateChanged += HandleStateChanged;
        HandleScoreChanged(GameManager.Instance.Score);
        HandleStateChanged(GameManager.Instance.CurrentState);
    }
    private void OnDestroy() {
        GameManager.Instance.OnScoreChanged -= HandleScoreChanged;
        GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    private void HandleScoreChanged(int newScore) {
        inGameScoreText.text = "Score: " + newScore;
        endGameScoreText.text = "Score: " + newScore;
    }

    private void HandleStateChanged(GameState newState) {
        gameOverUI.SetActive(newState == GameState.GameOver);
    }
}