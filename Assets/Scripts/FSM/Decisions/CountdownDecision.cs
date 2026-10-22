using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Countdown")]
public class CountdownDecision : Decision
{
    public float duration;

    public override bool Decide(StateController controller)
    {
        return controller.CheckIfCountDownElapsed(duration);
    }
}
