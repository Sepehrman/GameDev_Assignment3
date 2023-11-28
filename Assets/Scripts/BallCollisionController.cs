using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollisionController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource hitSoundEffect;
    Boolean hasPlayedSound = false;
    private MeshRenderer renderer;
    GameManager gameManager;



    private void Start() {
        gameManager = GameManager.Instance;
        renderer = GetComponent<MeshRenderer>();
    }



    void OnCollisionEnter(Collision other)
    {

        if (!hasPlayedSound) {
            hitSoundEffect.Play();
            hasPlayedSound = true;
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                renderer.enabled = false;
            }
        }
        StartCoroutine(DestroyBallOnDelay());
        
    }



    IEnumerator DestroyBallOnDelay() {

        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
