using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buttons : MonoBehaviour
{
    [Header("Play-Pause Buttons Highlight")]
    public Transform playButton;
    public Transform pauseButton;
    public Transform highlightPPB;

    [Header("Slider Value Texts")]
    public TextMeshProUGUI masterVolumeTMP;
    public TextMeshProUGUI sfxVolumeTMP;
    public TextMeshProUGUI musicVolumeTMP;

    public TextMeshProUGUI sfxPitchTMP;
    public TextMeshProUGUI musicPitchTMP;

    [Header("Music Buttons Highlight")]
    public List<Transform> musicButtons = new List<Transform>();
    public Transform marker;
    public TextMeshProUGUI musicTitleText;

    protected bool pauseButtonPressed;
    protected bool playerIsInTheEnd;
    protected bool musicStopped;

    protected List<Timer> timers = new List<Timer>();

    protected virtual void Start() => pauseButtonPressed = false;
    
    protected virtual void Update()
    {
        if (pauseButtonPressed) return;

        foreach (Timer timer in timers)
            if (timer != null || !timer.IsCompleted())
                timer.Tick(Time.deltaTime);
    }

    public virtual void PlaySoundEffect(int num) { if (pauseButtonPressed) return; }
    public virtual void PlayMusic(int num) 
    { 
        if (pauseButtonPressed) return;

        if (musicButtons.Count > 0 && num < 10 && num > 0)
        {
            marker.position = musicButtons[num - 1].position;
            playerIsInTheEnd = num < 6 ? false : true;
        }
    }


    #region Volume & Pitch Change
    //Volume Change
    public void masterVolumeChange(float value)
    {
        SoundManager.i.SetMasterVolume(value);
        masterVolumeTMP.text = ((int)(value * 100f)).ToString();
    }
    public void sfxVolumeChange(float value)
    {
        SoundManager.i.SetSFXVolume(value);
        sfxVolumeTMP.text = ((int)(value * 100f)).ToString();
    }
    public void musicVolumeChange(float value)
    {
        SoundManager.i.SetMusicVolume(value);
        musicVolumeTMP.text = ((int)(value * 100f)).ToString();
    }

    //Pitch Change
    public void sfxPitchChange(float value)
    {
        SoundManager.i.SetSFXPitch(value / 10);
        sfxPitchTMP.text = ((int)(value)).ToString();
    }
    public void musicPitchChange(float value)
    {
        SoundManager.i.SetMusicPitch(value / 10);
        musicPitchTMP.text = ((int)(value)).ToString();
    }
    #endregion
}
