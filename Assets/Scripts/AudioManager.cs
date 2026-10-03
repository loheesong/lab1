using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip overworldBgm;

    [Header("Common World SFX")]
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip brickBumpClip;

    private void Awake() {
        // Singleton pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        PlayDefaultBGM();
    }

    // ----------------- BGM Controls -----------------
    public void PlayDefaultBGM() {
        PlayMusic(overworldBgm);
    }

    public void PlayMusic(AudioClip clip, bool loop = true) {
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopMusic() {
        bgmSource.Stop();
    }

    public void PauseMusic() {
        bgmSource.Pause();
    }

    public void ResumeMusic() {
        bgmSource.UnPause();
    }

    // ----------------- World SFX -----------------
    public void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayCoinSound() => PlaySFX(coinClip);
    public void PlayBrickBumpSound() => PlaySFX(brickBumpClip);
}