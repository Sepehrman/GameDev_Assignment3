using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    // Initial positions for reset
    private Vector3 initialPosition;
    //private Quaternion initialRotation;
    private Vector3 initialRotation;

    public CapsuleCollider capsuleCollider;
    public bool canWalkThroughWalls = false;

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    void Update()
    {
        // Reset position when home key pressed
        if (Input.GetKeyDown(KeyCode.Home))
        {
            ResetPosition();
        }
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            if (canWalkThroughWalls)
            {
                Debug.Log("Adding wall to rigid body exclusian layers");
                //capsuleCollider.includeLayers |= (1 << LayerMask.NameToLayer("Wall"));// Adding
                //rigidbody.includeLayers |= (1 << LayerMask.NameToLayer("Wall"));

                capsuleCollider.excludeLayers &= ~(1 << LayerMask.NameToLayer("Wall")); // Removing
                rigidbody.excludeLayers &= ~(1 << LayerMask.NameToLayer("Wall"));

            } else
            {
                capsuleCollider.excludeLayers |= (1 << LayerMask.NameToLayer("Wall"));// Adding
                rigidbody.excludeLayers |= (1 << LayerMask.NameToLayer("Wall"));

                //capsuleCollider.excludeLayers = (1 << LayerMask.GetMask("Nothing"));
                //capsuleCollider.excludeLayers = ~0; // Everything
            }
            canWalkThroughWalls = !canWalkThroughWalls;
        }
         if (Input.GetKeyDown(KeyCode.O))
        {
            SaveLoadManager.slManager.SaveDefaultSlot();
            Debug.Log("Save");
        }
    
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoadManager.slManager.LoadSaveSlot();
            Debug.Log("Load");
        }
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }

    private void ResetPosition()
    {
        transform.position = initialPosition;
        //transform.rotation = initialRotation;
        transform.eulerAngles = initialRotation;
    }
}