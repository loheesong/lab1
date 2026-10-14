using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class HUDManager : MonoBehaviour {
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI inGameScoreText;
    [SerializeField] private TextMeshProUGUI endGameScoreText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private GameObject pauseUI;

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
    private void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) TogglePause();
    }

    private void HandleScoreChanged(int newScore) {
        inGameScoreText.text = "Score: " + newScore;
        endGameScoreText.text = "Score: " + newScore;
    }

    private void HandleStateChanged(GameState newState) {
        gameOverUI.SetActive(newState == GameState.GameOver);
        pauseButtonText.text = newState == GameState.Paused ? "UNPAUSE" : "PAUSE";
        pauseUI.SetActive(newState == GameState.Paused);
    }
    public void TogglePause() => GameManager.Instance.TogglePause();
    public void Restart() => GameManager.Instance.RestartGame();
    public void ReturnToMainMenu() => GameManager.Instance.ReturnToMainMenu();
}