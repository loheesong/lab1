using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Playing,
    GameOver
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

    [Header("Game References")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Transform obstaclesParent;

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
                Time.timeScale = 0.0f;
                break;
        }
        OnStateChanged?.Invoke(currentState);
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

    private void ResetObstacles() {
        foreach (Transform child in obstaclesParent) {
            if (child.TryGetComponent<BrickController>(out BrickController brick)) {
                brick.ResetBrick();
                continue;
            }

            QuestionBoxController qBox = child.GetComponentInChildren<QuestionBoxController>();
            if (qBox != null) qBox.ResetQuestionBox();
        }
    }

    // Called by the Restart Button
    public void RestartGame() {
        ResetScore();
        ResetObstacles();
        player.ResetPlayer();
        // Notify EnemyManager and other observers to reset
        OnGameRestart?.Invoke();
        SetGameState(GameState.Playing);
    }

    // change scene stuff 
    private void OnDestroy() {
        SceneManager.activeSceneChanged -= SceneSetup;
    }

    public void SceneSetup(Scene current, Scene next) {
        SetGameState(GameState.Playing);
        OnScoreChanged?.Invoke(score);
    }
}