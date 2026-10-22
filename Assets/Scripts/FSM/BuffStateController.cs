using UnityEngine;

public class BuffStateController : StateController {
    [Header("Active Buff Data")]
    public PowerupType currentBuffType = PowerupType.Default;
    public bool isInvincible = false;

    [Header("Audio")]
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip starmanBgm;
    [SerializeField] private AudioClip defaultBgm;

    public override void Start() {
        base.Start();
        GameRestart();
    }

    public void GameRestart() {
        currentBuffType = PowerupType.Default;
        isInvincible = false;
        if (bgmAudioSource != null && defaultBgm != null) {
            bgmAudioSource.clip = defaultBgm;
            bgmAudioSource.Play();
        }
        TransitionToState(startState);
    }

    public void SetBuff(PowerupType buff) {
        currentBuffType = buff;
    }

    public void StartStarman() {
        isInvincible = true;
        if (bgmAudioSource != null && starmanBgm != null) {
            bgmAudioSource.clip = starmanBgm;
            bgmAudioSource.Play();
        }
    }

    public void ExitStarman() {
        isInvincible = false;
        if (bgmAudioSource != null && defaultBgm != null) {
            bgmAudioSource.clip = defaultBgm;
            bgmAudioSource.Play();
        }
    }
}