using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public GameObject audioManager;  // Reference to self to destroy if needed

    public static AudioManager Instance { get; private set; }

    public AudioSource currentMusicTrack;
    public AudioSource nightMusicTrack;
    public AudioSource dayMusicTrack;

    private const float baseMusicVolumeLevel = 0.7f;
    public bool isFogOn = false;
    public bool isDay = true;
    public bool lastIsDay = true;
    public bool isMusicPaused = false;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(audioManager.gameObject);
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Debug.Log("audioManager Ready");
        currentMusicTrack = dayMusicTrack;
        if (currentMusicTrack)
        {
            currentMusicTrack.Play();
        }
    }

    protected void FixedUpdate()
    {
        // Toggle functionality for testing
        if (lastIsDay != isDay)
        {
            lastIsDay = isDay;
            currentMusicTrack?.Pause();
            if (isDay)
            {
                currentMusicTrack = dayMusicTrack;
            } else
            {
                currentMusicTrack = nightMusicTrack;
            }
            currentMusicTrack?.Play();
        }
    }

    public void setMusicTrackAudioLevel(float level)
    {
        float musicVolume = level * baseMusicVolumeLevel;   // Based On Distance
        musicVolume = isFogOn ? (musicVolume * 0.5f) : musicVolume; // Based On Fog
        if (currentMusicTrack)
        {
            currentMusicTrack.volume = musicVolume;
        }
    }

    public void setIsFogOn(bool value)
    {
        isFogOn = value;
    }

    // Toggles music between paused and unpaused
    public void toggleMusicPause()
    {
        if (isMusicPaused)
        {
            currentMusicTrack.Play();
        } else
        {
            currentMusicTrack.Pause();
        }
        isMusicPaused = !isMusicPaused;
    }

    // Set isDay and change music track accordingly
    public void setIsDay(bool value) {
        isDay = value;
        currentMusicTrack?.Pause();
        if (isDay)
        {
            currentMusicTrack = dayMusicTrack;
        }
        else
        {
            currentMusicTrack = nightMusicTrack;
        }
        currentMusicTrack?.Play();
    }
}
