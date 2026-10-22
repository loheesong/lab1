using UnityEngine;

public abstract class StateController : MonoBehaviour
{
    [Header("States")]
    public State startState;
    public State previousState;
    public State currentState;
    public State remainState;

    [HideInInspector] public bool transitionStateChanged = false;
    [HideInInspector] public float stateTimeElapsed;
    public bool isActive = true;

    public virtual void Start()
    {
        OnSetupState();
    }

    public virtual void OnSetupState()
    {
        if (currentState != null)
        {
            currentState.DoSetupActions(this);
        }
    }

    public virtual void OnExitState()
    {
        stateTimeElapsed = 0f;
        if (currentState != null)
        {
            currentState.DoExitActions(this);
        }
    }

    public virtual void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, 1.0f);
        }
    }

    public void TransitionToState(State nextState)
    {
        if (nextState == remainState || nextState == null) return;

        OnExitState();

        previousState = currentState;
        currentState = nextState;
        transitionStateChanged = true;

        OnSetupState();
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return stateTimeElapsed >= duration;
    }

    public virtual void Update()
    {
        if (!isActive || currentState == null) return;
        currentState.UpdateState(this);
    }
}