using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input Handler", menuName = "Input Handler")]
public class InputHandler : ScriptableObject, CustomInput.IGameplayActions {
    private CustomInput input;

    public UnityAction<float> OnMoveAction;
    public UnityAction<float> OnRotateAction;
    public UnityAction OnImmediateDropAction;
    public UnityAction OnFastDropStartAction;
    public UnityAction OnFastDropEndAction;

    public void OnEnable() {
        if (input == null) {
            input = new CustomInput();
        }

        input.Gameplay.SetCallbacks(this);

        input.Enable();
        input.Gameplay.Enable();
    }

    public void OnDisable() {
        input.Gameplay.Disable();
    }

    public void OnImmediate(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            OnImmediateDropAction?.Invoke();
        }
    }

    public void OnFast(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            OnFastDropStartAction?.Invoke();
        } else if (context.phase == InputActionPhase.Canceled) {
            OnFastDropEndAction?.Invoke();
        }
    }

    public void OnRotatePiece(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            OnRotateAction?.Invoke(context.ReadValue<float>());
        }
    }

    public void OnMovePiece(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled) {
            OnMoveAction?.Invoke(context.ReadValue<float>());
        }
    }
}
