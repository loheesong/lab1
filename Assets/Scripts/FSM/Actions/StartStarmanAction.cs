using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/StartStarman")]
public class StartStarmanAction : Action {
    public override void Act(StateController controller) {
        if (controller is BuffStateController buff) {
            buff.StartStarman();
        }
    }
}