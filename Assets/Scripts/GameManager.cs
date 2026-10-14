using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Playing,
    GameOver,
    Paused
}

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    [SerializeField] private GameState currentState = GameState.Playing;
    public GameState CurrentState => currentState;

    [Header("Score")]
    [SerializeField] private int score = 0;
    public int Score => score;
    [SerializeField] private IntVariable highScoreSO;
    public int HighScore => highScoreSO.Value;

    public event Action<int> OnScoreChanged;
    public event Action<GameState> OnStateChanged;
    public event Action OnGameRestart;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        SetGameState(GameState.Playing);
        OnScoreChanged?.Invoke(score);
        SceneManager.activeSceneChanged += SceneSetup;
    }

    public void SetGameState(GameState newState) {
        currentState = newState;

        switch (currentState) {
            case GameState.Playing:
                Time.timeScale = 1.0f;
                break;

            case GameState.GameOver:
            case GameState.Paused:
                Time.timeScale = 0.0f;
                break;
        }
        OnStateChanged?.Invoke(currentState);
    }
    public void TogglePause() {
        if (currentState == GameState.GameOver) return;

        if (currentState == GameState.Playing) {
            AudioManager.Instance.PauseMusic();
            SetGameState(GameState.Paused);
        } else {
            AudioManager.Instance.ResumeMusic();
            SetGameState(GameState.Playing);
        }
    }

    public void ReturnToMainMenu() {
        Time.timeScale = 1.0f;
        Destroy(AudioManager.Instance.gameObject);
        Destroy(gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void AddScore(int points) {
        if (currentState != GameState.Playing) return;

        score += points;
        OnScoreChanged?.Invoke(score);

        if (score > highScoreSO.Value) {
            highScoreSO.SetValue(score);
        }
    }

    public void ResetScore() {
        score = 0;
        OnScoreChanged?.Invoke(score);
    }

    // Called by the Restart Button
    public void RestartGame() {
        ResetScore();
        // Notify EnemyManager and other observers to reset
        OnGameRestart?.Invoke();
        SetGameState(GameState.Playing);
    }

    // change scene stuff 
    private void OnDestroy() {
        SceneManager.activeSceneChanged -= SceneSetup;
        if (Instance == this) Instance = null;
    }

    public void SceneSetup(Scene current, Scene next) {
        SetGameState(GameState.Playing);
        OnScoreChanged?.Invoke(score);
    }
}