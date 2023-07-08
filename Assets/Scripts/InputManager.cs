using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance {
        get { return instance; }
    }

    public delegate void ClickStarted(Vector2 position, float time);
    public event ClickStarted OnClickStarted;
    public delegate void ClickEnded(Vector2 position, float time);
    public event ClickEnded OnClickEnded;

    public delegate void RollPerformed();
    public event RollPerformed OnRollPerformed;
    public delegate void ReverseRollPerformed();
    public event ReverseRollPerformed OnReverseRollPerformed;

    private Input input;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        input = new Input();

        input.Player.Click.started += Click_Started;
        input.Player.Click.canceled += Click_Ended;
        input.Player.Roll.performed += Roll_Performed;
        input.Player.ReverseRoll.performed += Reverse_Roll_Performed;
    }

    private void Click_Started(InputAction.CallbackContext ctx) {
        //Debug.Log("click started");
        OnClickStarted?.Invoke(input.Player.MousePosition.ReadValue<Vector2>(), (float)ctx.startTime);
    }
    private void Click_Ended(InputAction.CallbackContext ctx) {
        //Debug.Log("click ended");
        OnClickEnded?.Invoke(input.Player.MousePosition.ReadValue<Vector2>(), (float)ctx.startTime);
    }
    private void Roll_Performed(InputAction.CallbackContext ctx) {
        //Debug.Log("Roll Performed");
        OnRollPerformed?.Invoke();
    }
    private void Reverse_Roll_Performed(InputAction.CallbackContext ctx) {
        //Debug.Log("Roll Performed");
        OnReverseRollPerformed?.Invoke();
    }
    private void OnEnable() {
        input.Player.Enable();
    }
    private void OnDisable() {
        input.Player.Disable();
    }
}
