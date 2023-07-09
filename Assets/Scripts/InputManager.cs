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
    public bool lockClick { get; set; }
    public bool lockRoll { get; set; }
    public bool lockReverseRoll { get; set; }
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
        if (lockClick) return;
        OnClickStarted?.Invoke(input.Player.MousePosition.ReadValue<Vector2>(), (float)ctx.startTime);
    }
    private void Click_Ended(InputAction.CallbackContext ctx) {
        //Debug.Log("click ended");
        if (lockClick) return;
        OnClickEnded?.Invoke(input.Player.MousePosition.ReadValue<Vector2>(), (float)ctx.startTime);
    }
    private void Roll_Performed(InputAction.CallbackContext ctx) {
        //Debug.Log("Roll Performed");
        if (lockRoll) return;
        OnRollPerformed?.Invoke();
    }
    private void Reverse_Roll_Performed(InputAction.CallbackContext ctx) {
        //Debug.Log("Roll Performed");
        if (lockReverseRoll) return;
        OnReverseRollPerformed?.Invoke();
    }
    private void OnEnable() {
        input.Player.Enable();
    }
    private void OnDisable() {
        input.Player.Disable();
    }
    public void lockAll(bool locks) {
        lockClick = locks;
        lockRoll = locks;
        lockReverseRoll = locks;
    }
}
