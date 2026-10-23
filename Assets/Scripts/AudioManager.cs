using UnityEngine;

public class AudioManager : MonoBehaviour {
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip overworldBgm;
    [SerializeField] private AudioClip pauseClip;

    [Header("Common World SFX")]
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip brickBumpClip;
    [SerializeField] private AudioClip powerupAppearClip;
    [SerializeField] private AudioClip powerupCollectClip;
    [SerializeField] private AudioClip starmanBgm;
    [SerializeField] private AudioClip gameOverClip;
    public void PlayStarmanBGM() => PlayMusic(starmanBgm);
    public void PlayGameOver() => PlaySFX(gameOverClip);

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
        PlaySFX(pauseClip);
    }

    public void ResumeMusic() {
        bgmSource.UnPause();
        PlaySFX(pauseClip);
    }

    // ----------------- World SFX -----------------
    public void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayCoinSound() => PlaySFX(coinClip);
    public void PlayBrickBumpSound() => PlaySFX(brickBumpClip);
    public void PlayPowerupAppearSound() => PlaySFX(powerupAppearClip);
    public void PlayPowerupSound() => PlaySFX(powerupCollectClip);
}