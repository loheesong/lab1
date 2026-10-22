using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class HUDManager : MonoBehaviour {
    [Header("Shared Variable Reference")]
    [SerializeField] private IntVariable currentScoreSO;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI inGameScoreText;
    [SerializeField] private TextMeshProUGUI endGameScoreText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private GameObject pauseUI;

    [Header("Scene Controller")]
    [SerializeField] private GameManager sceneGameManager;

    private void Start() {
        UpdateScoreDisplay(currentScoreSO.Value);
    }

    private void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) TogglePause();
    }

    private void UpdateScoreDisplay(int newScore) {
        inGameScoreText.text = "Score: " + newScore;
        endGameScoreText.text = "Score: " + newScore;
    }

    public void HandleGameOver() {
        gameOverUI.SetActive(true);
    }

    public void HandlePauseState(bool isPaused) {
        pauseButtonText.text = isPaused ? "UNPAUSE" : "PAUSE";
        pauseUI.SetActive(isPaused);
    }

    public void SetPausedUI() => HandlePauseState(true);
    public void SetUnpausedUI() => HandlePauseState(false);
    public void TogglePause() {
        sceneGameManager.TogglePause();
    }
    public void Restart() {
        sceneGameManager.RestartGame();
    }
    public void ReturnToMainMenu() {
        sceneGameManager.ReturnToMainMenu();
    }

}