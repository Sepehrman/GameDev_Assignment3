using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerController : MonoBehaviour
{
    public Transform targetBall; // Assign the target ball in the Inspector
    public float moveSpeed = 1.3f; // Adjust the movement speed as needed

    void Update()
    {
        if (targetBall != null)
        {
            // Calculate the direction from this object to the target ball
            Vector3 directionToBall = targetBall.position - transform.position;

            // Calculate the relative velocity of the ball towards the AI player
            Vector3 relativeVelocity = targetBall.GetComponent<Rigidbody>().velocity;

            // Check if the ball is moving directly towards the AI player
            if (Vector3.Dot(relativeVelocity.normalized, directionToBall.normalized) < 0.9f)
            {
                transform.Translate(new Vector3(0, 0, directionToBall.z).normalized * moveSpeed * Time.deltaTime);
            }

        }
    }
}
