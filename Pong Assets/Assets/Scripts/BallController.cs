using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public float initialForceMagnitude = 15f; // Adjust the force magnitude as needed
    public float ballSpeed = 13f;
    public Vector3 initialForceDirection = Vector3.left; // Adjust the force direction as needed
    public float bounceForce = 25f;
    public int collisionCounter = 0;
    private Rigidbody rb;
    public TMP_Text playerOneScore;
    public TMP_Text playerTwoScore;
    public int p1Score;
    public int p2Score;
    public Vector3 initialPosition;
    public Vector3 forceDirection = Vector3.forward;
    public float consecutiveCollisionForce = 30f;
    public int consecutiveCollisionThreshold = 5;
    public AIAgent aIAgent;

    void Start()
    {
        initialPosition = gameObject.transform.position;
        Vector3 initialForceDirection = RandomizeDirection();
        rb = GetComponent<Rigidbody>();
        rb.AddForce(initialForceDirection * ballSpeed, ForceMode.VelocityChange);
    }


    
    private void OnTriggerEnter(Collider other) {
        
        RespawnBall();
        if (other.gameObject.CompareTag("Goal-P1") && playerTwoScore != null) {
            p2Score = int.Parse(playerTwoScore.text);
            p2Score++;
            playerTwoScore.text = p2Score.ToString();
        } else if (other.gameObject.CompareTag("Goal-P2") && playerOneScore != null) {
            p1Score = int.Parse(playerOneScore.text);
            p1Score++;
            playerOneScore.text = p1Score.ToString();
        }
    }

    private void RespawnBall() {
        gameObject.transform.position = initialPosition;
        // Reverses the direction the ball goes for
        rb.AddForce(initialForceDirection * ballSpeed, ForceMode.VelocityChange);
    }
    
    private void FreezeBall() {

    }

    private Vector3 RandomizeDirection() {
        float randomX = Random.Range(0, 2) == 0 ? -1 : 1;
        float randomZ = Random.Range(0, 2) == 0 ? -1 : 1;
        return new Vector3(-0.5f * randomX, 0, -0.5f * randomZ).normalized;
    }

    void Update()
    {
        if (rb.velocity != Vector3.zero)
        {
            rb.velocity = rb.velocity.normalized * ballSpeed;
        }
    }

}