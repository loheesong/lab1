using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Decisions/BuffTransform")]
public class BuffTransformDecision : Decision
{
    public PowerupType targetBuff = PowerupType.StarMan;

    public override bool Decide(StateController controller)
    {
        if (controller is BuffStateController buff)
        {
            return buff.currentBuffType == targetBuff;
        }
        return false;
    }
}