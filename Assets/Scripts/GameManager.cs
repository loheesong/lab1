using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Playing,
    GameOver,
    Paused
}

public class GameManager : MonoBehaviour {

    [Header("Game State")]
    [SerializeField] private GameState currentState = GameState.Playing;
    public GameState CurrentState => currentState;

    [Header("Score")]
    [SerializeField] private IntVariable currentScoreSO;
    [SerializeField] private IntVariable highScoreSO;

    [Header("Broadcast Events")]
    [SerializeField] private IntGameEvent onScoreChanged;
    [SerializeField] private SimpleGameEvent onGameRestart;
    [SerializeField] private SimpleGameEvent onGameOver;
    [SerializeField] private SimpleGameEvent onGamePause;
    [SerializeField] private SimpleGameEvent onGameResume;

    private void Start() {
        SetGameState(GameState.Playing);
        onScoreChanged.Raise(currentScoreSO.Value);
    }

    public void SetGameState(GameState newState) {
        currentState = newState;

        switch (currentState) {
            case GameState.Playing:
                Time.timeScale = 1.0f;
                onGameResume?.Raise();
                break;
            case GameState.GameOver:
                Time.timeScale = 0.0f;
                onGameOver?.Raise();
                break;
            case GameState.Paused:
                Time.timeScale = 0.0f;
                onGamePause?.Raise();
                break;
        }
    }

    public void TogglePause() {
        if (currentState == GameState.GameOver) return;

        if (currentState == GameState.Playing) {
            SetGameState(GameState.Paused);
        } else {
            SetGameState(GameState.Playing);
        }
    }

    public void ReturnToMainMenu() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void AddScore(int points) {
        if (currentState != GameState.Playing) return;
        currentScoreSO.ApplyChange(points);
        onScoreChanged?.Raise(currentScoreSO.Value);
        if (currentScoreSO.Value > highScoreSO.Value) highScoreSO.SetValue(currentScoreSO.Value);
    }

    // Called by the Restart Button
    public void RestartGame() {
        currentScoreSO.ResetValue();
        onScoreChanged?.Raise(currentScoreSO.Value);
        onGameRestart?.Raise();
        SetGameState(GameState.Playing);
    }
}