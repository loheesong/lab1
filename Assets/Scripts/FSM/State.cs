using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/State")]
public class State : ScriptableObject
{
    public Action[] setupActions;
    public Action[] actions;
    public EventAction[] eventTriggeredActions;
    public Action[] exitActions;
    public Transition[] transitions;

    public Color sceneGizmoColor = Color.grey;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    protected void DoActions(StateController controller)
    {
        if (actions == null) return;
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    public void DoSetupActions(StateController controller)
    {
        if (setupActions == null) return;
        for (int i = 0; i < setupActions.Length; i++)
        {
            setupActions[i].Act(controller);
        }
    }

    public void DoExitActions(StateController controller)
    {
        if (exitActions == null) return;
        for (int i = 0; i < exitActions.Length; i++)
        {
            exitActions[i].Act(controller);
        }
    }

    public void DoEventTriggeredActions(StateController controller, ActionType type = ActionType.Default)
    {
        if (eventTriggeredActions == null) return;
        foreach (EventAction eventTriggeredAction in eventTriggeredActions)
        {
            if (eventTriggeredAction.type == type && eventTriggeredAction.action != null)
            {
                eventTriggeredAction.action.Act(controller);
            }
        }
    }

    protected void CheckTransitions(StateController controller)
    {
        controller.transitionStateChanged = false;
        if (transitions == null) return;

        for (int i = 0; i < transitions.Length; ++i)
        {
            if (controller.transitionStateChanged)
            {
                break;
            }
            if (transitions[i].decision == null) continue;

            bool decisionSucceeded = transitions[i].decision.Decide(controller);
            if (decisionSucceeded)
            {
                controller.TransitionToState(transitions[i].trueState);
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
}