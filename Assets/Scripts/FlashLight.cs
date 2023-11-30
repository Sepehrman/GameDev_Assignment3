using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source: https://www.youtube.com/watch?v=EFt_lLVDeRo&ab_channel=Acerola

public class FlashLight : MonoBehaviour {
    [Header("Fog")]
    public Shader fogShader;
    public Color fogColor;
    
    [Range(0.0f, 1.0f)]
    public float fogDensity;
    
    [Range(0.0f, 50.0f)]
    public float fogOffset;

    private float oldFogValue;

    void Start() {
     

        fogOffset = 0f;
        fogDensity = 0f;
        oldFogValue = fogDensity;
    }

    [ImageEffectOpaque]

    public void ToggleFog(){
        if(fogDensity==0){
            fogDensity = oldFogValue;
        }else{
            oldFogValue = fogDensity;
            fogDensity = 0;
        }
    }
}