using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Controller : MonoBehaviour
{
    //create private internal references
    private P2InputActions inputActions;
    private InputAction movement;
    public float bounceForce = 10f;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); //get rigidbody, responsible for enabling collision with other colliders
        inputActions = new P2InputActions(); //create new InputActions
    }
    
    private void OnEnable()
    {
        movement = inputActions.Player2.Movement; //get reference to movement action
        movement.Enable();

    }



    //called when script disabled
    private void OnDisable()
    {
        movement.Disable();
        // inputActions.Player.Jump.Disable();
    }

    //called every physics update
    private void FixedUpdate()
    {
        Vector2 v2 = movement.ReadValue<Vector2>(); //extract 2d input data
        Vector3 v3 = new Vector3(v2.x, 0, v2.y); //convert to 3d space

        //transform.Translate(v3); //moves transform, ignoring physics
        rb.AddForce(v3, ForceMode.VelocityChange); //apply instant physics force, ignoring mass
    }
}
