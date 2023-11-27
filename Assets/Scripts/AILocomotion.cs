using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on code from TheKiwiCoder: https://www.youtube.com/watch?v=_I8HsTfKep8&t=1s
//Standard assets for animation
//Mixamo for Pistol idle animation
//POLYGON starter pack https://assetstore.unity.com/packages/3d/props/polygon-starter-pack-low-poly-3d-art-by-synty-156819?aid=1011ljjCh&utm_campaign=unity_affiliate&utm_medium=affiliate&utm_source=partnerize-linkmaker
public class AILocomotion : MonoBehaviour
{
    Animator animator;
    public GameObject agentGameobject;  // Parent
    AIAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get Agent velocity
        UnityEngine.AI.NavMeshAgent agent = agentGameobject.GetComponent<AIAgent>().agent;
        Vector3 velocity = agent.velocity;
        Vector3 relativeVelocity = Quaternion.Inverse(agent.transform.rotation) * velocity;

        animator.SetFloat("InputX", relativeVelocity.x);
        animator.SetFloat("InputY", relativeVelocity.z);
        //animator.SetFloat("InputY", 1);       // Run forward animation
    }
}
