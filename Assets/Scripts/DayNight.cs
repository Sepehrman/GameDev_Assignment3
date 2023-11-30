using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    public Material sky;
    public Light sun;

    public Fog fog;
    // Start is called before the first frame update


    void Start(){

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            // Toggle between day and night on each key press
            ToggleDay();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Toggle between day and night on each key press
            ToggleNight();
        }
           if (Input.GetKeyDown(KeyCode.M))
        {
            // Toggle between day and night on each key press
            fog.ToggleFog();
        }
    }

    void ToggleDay(){
        sky.SetColor("_SkyTint", Color.white);
        RenderSettings.skybox.SetFloat("_Exposure", 2f);
        sun.color = Color.yellow;
        sun.intensity = 2f;
        AudioManager.Instance.setIsDay(true);
    }

    void ToggleNight(){
        sky.SetColor("_SkyTint", Color.black);
        RenderSettings.skybox.SetFloat("_Exposure", 0.2f);
        sun.color = Color.red;
        sun.intensity = 1f;
        AudioManager.Instance.setIsDay(false);
    }
}
