using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Source: https://www.youtube.com/watch?v=EFt_lLVDeRo&ab_channel=Acerola

[RequireComponent(typeof(Camera))]
public class Fog : MonoBehaviour {
    [Header("Fog")]
    public Shader fogShader;
    public Color fogColor;
    
    [Range(0.0f, 1.0f)]
    public float fogDensity;
    
    [Range(0.0f, 50.0f)]
    public float fogOffset;
    
    private Material fogMat;

    private float oldFogValue;

    void Start() {
        if (fogMat == null) {
            fogMat = new Material(fogShader);
            fogMat.hideFlags = HideFlags.HideAndDontSave;
        }

        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
        fogOffset = 5f;
        fogDensity = 0.3f;
        oldFogValue = fogDensity;
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        fogMat.SetVector("_FogColor", fogColor);
        fogMat.SetFloat("_FogDensity", fogDensity);
        fogMat.SetFloat("_FogOffset", fogOffset);
        Graphics.Blit(source, destination, fogMat);
    }

    public void ToggleFog(){
        if(fogDensity==0){
            fogDensity = oldFogValue;
        }else{
            oldFogValue = fogDensity;
            fogDensity = 0;
        }
    }
}