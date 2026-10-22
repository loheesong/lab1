using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/SetupAnimator")]
public class SetupAnimatorAction : Action {
    [Tooltip("The RuntimeAnimatorController to assign to Mario when entering this state.")]
    public RuntimeAnimatorController animatorController;

    public override void Act(StateController controller) {
        if (animatorController == null) {
            Debug.LogWarning($"[SetupAnimatorAction] No RuntimeAnimatorController assigned on {name}.", this);
            return;
        }

        // Try getting the Animator component on the controller or its children
        Animator animator = controller.GetComponent<Animator>();
        if (animator == null) {
            animator = controller.GetComponentInChildren<Animator>();
        }

        if (animator != null) {
            animator.runtimeAnimatorController = animatorController;
        } else {
            Debug.LogError($"[SetupAnimatorAction] Could not find an Animator component on {controller.gameObject.name}.", controller);
        }
    }
}