using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private InputManager input;
    private InputActions inputActions;
    public float mouseSensitivity = 40f;
    private Vector2 mouseLook;
    private float xRotation = 0f;
    public Transform playerBody;
    public Transform playerCamera;
    public float moveSpeed = 5.0f;
    private LayerMask mask;
    private Rigidbody rb; // Fixed typo in the variable name
    private bool canWalkThroughWalls;
    private CapsuleCollider capsuleCollider; // Reference to CapsuleCollider
    [SerializeField]
    private GameObject throwingBall;
    [SerializeField]
    private GameObject throwingPoint;

    // Initial positions for reset
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    public GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Fix the typo in getting the Rigidbody component
        playerBody = transform;
        capsuleCollider = GetComponent<CapsuleCollider>(); // Get the CapsuleCollider component
        input = GetComponent<InputManager>(); // Reference the existing InputManager component
        playerCamera = Camera.main.transform; // Get the main camera
        Cursor.lockState = CursorLockMode.Locked;

        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    void Update()
    {
        Look();
        Move();


        // Code for adding/Removing the wall layer
        if (input.Invisible)
        {
            if (canWalkThroughWalls)
            {
                capsuleCollider.excludeLayers &= ~(1 << LayerMask.NameToLayer("Wall")); // Remove Layers
                GetComponent<Rigidbody>().excludeLayers &= ~(1 << LayerMask.NameToLayer("Wall"));
            } 
            else
            {
                capsuleCollider.excludeLayers |= (1 << LayerMask.NameToLayer("Wall"));// Adding Layers
                GetComponent<Rigidbody>().excludeLayers |= (1 << LayerMask.NameToLayer("Wall"));
            }
            canWalkThroughWalls = !canWalkThroughWalls;
        }

        if (input.Shoot) {
            Shoot();
        }

        if (input.Reset)
        {
            ResetPosition();
            GameManager.Instance.ResetEnemyPosition();
        }

        if (input.Jump)
        {
            Jump();
        }
    }

    void Look()
    {
        mouseLook = input.MouseLook; // Access MouseLook from InputManager

        float mouseX = mouseLook.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseLook.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    void Move()
    {
        Vector2 movementInput = input.Move; // Access Move from InputManager
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime;
        moveDirection = transform.TransformDirection(moveDirection);
        transform.position += moveDirection;
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

    // Resets player to initial position
    private void ResetPosition()
    {
        transform.position = initialPosition;
        transform.eulerAngles = initialRotation;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            collision.gameObject.SetActive(false);
        } else if (collision.gameObject.tag == "Enemy") {
            Debug.Log("Die");
            gameManager.Restart();
        }
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Shoot() {
        Vector3 cameraForward = playerCamera.forward;
        GameObject bullet = Instantiate(throwingBall, throwingPoint.transform.position, Quaternion.LookRotation(cameraForward));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(cameraForward * 15, ForceMode.Impulse);
    }


}
