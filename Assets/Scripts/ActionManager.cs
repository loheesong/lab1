using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ActionManager : MonoBehaviour {
    [Header("UnityEvent Subjects")]
    public UnityEvent jump;
    public UnityEvent<int> moveCheck;

    public void OnMoveAction(InputAction.CallbackContext context) {
        if (context.started) {
            float val = context.ReadValue<float>();
            moveCheck.Invoke(val > 0 ? 1 : -1);
        } else if (context.canceled) {
            moveCheck.Invoke(0);
        }
    }

    public void OnJumpAction(InputAction.CallbackContext context) {
        if (context.performed) jump.Invoke();
    }
}