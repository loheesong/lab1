using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/StartStarman")]
public class StartStarmanAction : Action {
    [Tooltip("Animator controller shown while Starman is active (e.g. FireMario).")]
    public RuntimeAnimatorController starmanAnimator;

    public override void Act(StateController controller) {
        if (controller is BuffStateController buff) {
            buff.StartStarman(starmanAnimator);
        }
    }
}