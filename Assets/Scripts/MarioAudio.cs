using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerMovement))]
public class MarioAudio : MonoBehaviour {
    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip deathClip;

    [SerializeField] private SimpleGameEvent onMarioDied;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJump() {
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlayDeath() {
        audioSource.Stop();
        audioSource.PlayOneShot(deathClip);
        onMarioDied?.Raise();
    }

    private void RestartMusic() {

    }
}