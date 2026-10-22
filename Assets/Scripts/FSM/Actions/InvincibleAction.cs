using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/SetupInvincibility")]
public class InvincibleAction : Action {
    [Tooltip("SFX played when taking damage / entering invincibility (e.g. smb_pipe_powerdown.wav).")]
    public AudioClip invincibilityStart;

    public override void Act(StateController controller) {
        if (controller is MarioStateController mario) {
            // Play the pipe powerdown sound
            AudioSource audioSource = mario.GetComponent<AudioSource>();
            if (audioSource != null && invincibilityStart != null) {
                audioSource.PlayOneShot(invincibilityStart);
            }

            // Trigger Mario's sprite renderer flickering coroutine
            mario.SetRendererToFlicker();
        }
    }
}