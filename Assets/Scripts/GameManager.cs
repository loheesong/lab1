using System;
using UnityEngine;
using TMPro;

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

    public event Action<int> OnScoreChanged;
    public event Action<GameState> OnStateChanged;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverUI;

    [Header("Game References")]
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Transform enemiesParent;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        SetGameState(GameState.Playing);
        UpdateScoreUI();
    }

    public void SetGameState(GameState newState) {
        currentState = newState;
        OnStateChanged?.Invoke(currentState);

        switch (currentState) {
            case GameState.Playing:
                Time.timeScale = 1.0f;
                if (gameOverUI != null) gameOverUI.SetActive(false);
                break;

            case GameState.GameOver:
                Time.timeScale = 0.0f;
                if (gameOverUI != null) gameOverUI.SetActive(true);
                break;
        }
    }

    public void AddScore(int points) {
        if (currentState != GameState.Playing) return;

        score += points;
        UpdateScoreUI();
        OnScoreChanged?.Invoke(score);
    }

    public void ResetScore() {
        score = 0;
        UpdateScoreUI();
        OnScoreChanged?.Invoke(score);
    }

    private void UpdateScoreUI() {
        if (scoreText != null) {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Called by the Restart Button
    public void RestartGame() {
        ResetScore();

        // Reset Goombas

        foreach (Transform enemy in enemiesParent) {
            EnemyMovement em = enemy.GetComponent<EnemyMovement>();
            em.ResetGoomba();
        }

        // Reset Player
        player.ResetPlayer();

        SetGameState(GameState.Playing);
    }
}