using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doortrigger : MonoBehaviour
{
void OnTriggerEnter(Collider ChangeScene) // can be Collider
{
    if(ChangeScene.gameObject.CompareTag("Player"))
    {
        Application.LoadLevel("PongMiniGame");
    }
}
}
