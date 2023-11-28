using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;



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
    private int health = 3;
    private List<GameObject> gridPositions = new List<GameObject>();
    private Boolean hasBlocks = false;
    private Boolean isDisappeared = false;
    public AudioSource deathSound;
    public AudioSource spawnSound;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
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
        if (!isDisappeared) {
            Patroling();
        }
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


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "ThrowingBall") {
            gameManager.AddScore();
            health--;

            if (health == 0) {
                
                if (!hasBlocks && gridPositions.Count == 0) {
                    SeparateBlocks();
                }

                Vector3 disappearPosition = new Vector3(gameObject.transform.position.x, -50, gameObject.transform.position.z);
                agentInitialPosition = disappearPosition;
                deathSound.Play();
                ResetPosition();

                isDisappeared = true;
                StartCoroutine(DisappearForFive());

            }

        }        
    }


    private IEnumerator DisappearForFive() {


        yield return new WaitForSeconds(5);

        health = 3;
        isDisappeared = false;

        System.Random rnd = new System.Random();
        int position = rnd.Next(gridPositions.Count);
        agentInitialPosition = gridPositions[position].transform.position;
        spawnSound.Play();
        ResetPosition();

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

    private void SeparateBlocks() {

        // List<GameObject> grids = new List<GameObject>();
        Scene currentScene = SceneManager.GetActiveScene();
        GameObject[] allGameObjects = currentScene.GetRootGameObjects();

        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name == "MazeBlock 1(Clone)") {
                
                // Vector3 newPosition = new Vector3(obj.transform.position.x, 30, obj.transform.position.z);
                // obj.transform.position = newPosition;

                gridPositions.Add(obj);
            }
        }
        hasBlocks = !hasBlocks;
    }
}
