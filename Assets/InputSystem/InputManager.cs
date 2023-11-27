using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private InputActions inputActions;
    public Vector2 Move => inputActions.Player.Movement.ReadValue<Vector2>();
    public bool Jump => inputActions.Player.Jump.triggered;
    public bool Invisible => inputActions.Player.Invisible.triggered;
    public bool Reset => inputActions.Player.Reset.triggered;
    // public bool Shoot => inputActions.Player.Shoot.triggered;
    public Vector2 MouseLook => inputActions.Player.Look.ReadValue<Vector2>();

    void Start()
    {

    }

    private void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}
