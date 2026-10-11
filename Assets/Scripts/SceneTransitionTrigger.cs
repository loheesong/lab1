using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour {
    [SerializeField] private string targetScene = "Scene2";

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            SceneManager.LoadScene(targetScene);
        }
    }
}
