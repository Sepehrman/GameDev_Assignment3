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



    private void Start() {
        renderer = GetComponent<MeshRenderer>();
    }



    void OnCollisionEnter(Collision other)
    {
        // Debug.Log(other.gameObject.name);

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
