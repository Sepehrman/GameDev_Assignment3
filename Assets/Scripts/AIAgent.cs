using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class AIAgent : MonoBehaviour
{

    public NavMeshAgent agent;
    public LayerMask whatIsGroud, whatIsPlayer;

    public float distaceFromWalkpoint;
    public float stopWithinDistanceOfWalkpoint = 2f;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    private Vector3 initialPosition;

    // Initial positions for reset
    private Vector3 agentInitialPosition;
    private Quaternion agentInitialRotation;

    void Start()
    {
        initialPosition = transform.position;

        agentInitialPosition = transform.position;
        agentInitialRotation = transform.rotation;
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Reset position when home key pressed
        if (Input.GetKeyDown(KeyCode.Home))
        {
            walkPointSet = false;   // Find new walkpoint
            ResetPosition();
        }
        Patroling();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        distaceFromWalkpoint = Vector3.Distance(transform.position, walkPoint);

        if (Vector3.Distance(transform.position, walkPoint) < stopWithinDistanceOfWalkpoint)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkPointRange;

        walkPoint = randomDirection + transform.position;
        NavMeshHit hit;
        walkPointSet = NavMesh.SamplePosition(walkPoint, out hit, 1.0f, NavMesh.AllAreas);

        if (walkPointSet)
        {
            Debug.DrawRay(hit.position, Vector3.up, Color.blue, 1.0f);

            // Check if path is reachable
            NavMeshPath pathTest = new NavMeshPath();
            agent.CalculatePath(hit.position, pathTest);
            walkPointSet = pathTest.status == NavMeshPathStatus.PathComplete;
        }
    }

    public void ResetPosition()
    {
        agent.Warp(agentInitialPosition);   // Transform equvilent for NavMeshAgent
    }
}
