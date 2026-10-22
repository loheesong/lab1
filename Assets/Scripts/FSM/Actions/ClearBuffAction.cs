using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/ClearBuff")]
public class ClearBuffAction : Action {
    public override void Act(StateController controller) {
        if (controller is BuffStateController buff) {
            buff.currentBuffType = PowerupType.Default;
        }
    }
}