using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour {
    [SerializeField] private Slider progressBar;
    private Coroutine loadRoutine;

    void Start() {
        loadRoutine = StartCoroutine(LoadingRoutine());
    }

    private IEnumerator LoadingRoutine() {
        float elapsed = 0f;
        float duration = 2f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            progressBar.value = elapsed / duration;
            yield return null;
        }

        SceneManager.LoadScene("SampleScene");
    }

    public void BackToMenu() {
        StopCoroutine(loadRoutine);
        SceneManager.LoadScene("MainMenu");
    }
}
