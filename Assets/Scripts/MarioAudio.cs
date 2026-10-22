using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerMovement))]
public class MarioAudio : MonoBehaviour {
    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deathClip;

    private AudioSource audioSource;
    private PlayerMovement playerMovement;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void PlayJump() {
        audioSource.PlayOneShot(jumpClip);
    }

    private void PlayDeath() {
        AudioManager.Instance.StopMusic();
        audioSource.Stop();
        audioSource.PlayOneShot(deathClip);
    }

    private void RestartMusic() {
        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlayDefaultBGM();
        }
    }
}