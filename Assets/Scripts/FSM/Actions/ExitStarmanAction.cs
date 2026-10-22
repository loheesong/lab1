using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/ExitStarman")]
public class ExitStarmanAction : Action {
    public override void Act(StateController controller) {
        if (controller is BuffStateController buff) {
            buff.ExitStarman();
        }
    }
}