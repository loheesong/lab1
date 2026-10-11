using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour {
    [SerializeField] private IntVariable highScoreSO;
    [SerializeField] private TextMeshProUGUI highScoreText;

    void Start() {
        highScoreText.text = "TOP: " + highScoreSO.Value;
    }

    public void PlayGame() {
        SceneManager.LoadScene("Loading");
    }

    public void ResetHighScore() {
        highScoreSO.ResetValue();
        highScoreText.text = "TOP: " + highScoreSO.Value;
    }
}
