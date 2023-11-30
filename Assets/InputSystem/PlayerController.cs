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
    public AudioSource wallCollisionAudio;
    public AudioSource footStepAudio;

    public GameObject flashLight;
    private bool flashlightToggle = true;

    public bool canEnterDoor = true;
    private int doorCooldown = 5;

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

        StartCoroutine(startDoorCooldown());
    }

    void Update()
    {

        Look();
        Move();


        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioManager.Instance.toggleMusicPause();
        }

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            flashlightToggle = !flashlightToggle;
            flashLight.SetActive(flashlightToggle);
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

        Vector3 oldPlayerPosition = transform.position;
        Vector2 movementInput = input.Move; // Access Move from InputManager
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y) * moveSpeed * Time.deltaTime;
        moveDirection = transform.TransformDirection(moveDirection);
        transform.position += moveDirection;
        if (Vector3.Distance(transform.position, oldPlayerPosition) < 0.0001f)
        {
            if (footStepAudio.isPlaying)
            {
                footStepAudio.Pause();
            }
        }
        else if (!footStepAudio.isPlaying && transform.position.y <= -3.5)
        {
            footStepAudio.Play();
        }
        else if (footStepAudio.isPlaying && transform.position.y > -3.5)
        {
            footStepAudio.Stop();
        }
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
            GameManager.Instance.Restart();
        }
        
        if (LayerMask.NameToLayer("Wall") == collision.gameObject.layer)
        {
            if (!wallCollisionAudio.isPlaying)
            {
                wallCollisionAudio.Play();
            }
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

    IEnumerator startDoorCooldown(){
        canEnterDoor = false;
        yield return new WaitForSeconds(doorCooldown);
        canEnterDoor = true;
    }

    void OnTriggerEnter(Collider ChangeScene) // can be Collider
    {
        if(ChangeScene.gameObject.CompareTag("Door"))
        {
            if(canEnterDoor){
            SaveLoadManager.slManager.SaveDefaultSlot();
            Application.LoadLevel("PongMiniGame");
            }
        }
    }


}
