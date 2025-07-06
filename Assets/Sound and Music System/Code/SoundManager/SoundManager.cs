using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager i;

    [Header("References")]
    [Header("CustomIncstuctions")]
    [SerializeField] private CustomizedSoundManagerBase customIncstuctions;

    [Header("Sound")]
    [SerializeField] private SoundLibrary soundLibrary;
    [Header("Music")]
    [SerializeField] public MusicLibrary musicLibrary;
    [SerializeField] public AudioSource musicEvent;
    [Header("Mixer")]
    [SerializeField] public AudioMixer audioMixer;


    [Header("Settings")]
    [Header("Volume")]
    [SerializeField] [Range(0.0001f, 1f)] public float masterVolume = 1;
    [SerializeField] [Range(0.0001f, 1f)] public float SFXVolume = 1;
    [SerializeField] [Range(0.0001f, 1f)] public float musicVolume = 1;

    [Header("Pitch")]
    [SerializeField] [Range(0f, 10f)] public float musicPitch = 1;
    [SerializeField] [Range(0f, 10f)] public float SFXPitch = 1;
    [HideInInspector] [Range(0f, 10f)] public float masterPitch = 1;

    [HideInInspector] public SoundControls soundControls;
    [HideInInspector] public MusicControls musicControls;


    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }

        soundControls = new SoundControls(soundLibrary, musicPitch);
    }

    void Start()
    {
        //Settings
        SetMasterVolumeInMixer(masterVolume);
        SetMusicVolumeInMixer(masterVolume);
        SetSFXVolumeInMixer(masterVolume);

        SetMusicPitchInMixer(musicPitch);
        SetSFXPitchInMixer(SFXPitch);

        CreateMusicControls();
    }

    void Update()
    {
        soundControls.UpdateSoundEvents();
        musicControls.UpdateMusic();
    }

    private void CreateMusicControls()
    {
        AudioSource trackObject1 = Instantiate(musicEvent, gameObject.transform);
        AudioSource trackObject2 = Instantiate(musicEvent, gameObject.transform);

        Func<MusicStates, MusicStates> cuctomFunc;
        if (customIncstuctions == null)
            cuctomFunc = new CustomizedSoundManagerBase().GetNextMusicState;
        else 
            cuctomFunc = customIncstuctions.GetNextMusicState;

        musicControls = new MusicControls(musicLibrary, musicEvent, musicPitch, trackObject1, trackObject2, cuctomFunc);
    }

    #region Settings
    public void SetMasterVolume(float value)
    {
        masterVolume = Mathf.Clamp(value, 0.0001f, 1f);
        SetMasterVolumeInMixer(masterVolume);
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp(value, 0.0001f, 1f);
        SetMusicVolumeInMixer(musicVolume);
    }
    public void SetSFXVolume(float value)
    {
        SFXVolume = Mathf.Clamp(value, 0.0001f, 1f);
        SetSFXVolumeInMixer(SFXVolume);
    }

    private void SetMasterVolumeInMixer(float value)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(value) * 20f);
    }
    private void SetMusicVolumeInMixer(float value)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(value) * 20f);
    }
    private void SetSFXVolumeInMixer(float value)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(value) * 20f);
    }

    private void SetMusicLowPass(float value)
    {
        audioMixer.SetFloat("musicLowPass", value);
    }


    /*public void SetMasterPitch(float value)
    { //Do not use!
        masterPitch = Mathf.Clamp(value, 0f, 100f);
        SetMasterPitchInMixer(masterPitch);
    }*/
    public void SetMusicPitch(float value)
    {
        musicPitch = Mathf.Clamp(value, 0f, 100f);
        
        SetMusicPitchInMixer(musicPitch);
    }
    public void SetSFXPitch(float value)
    {
        SFXPitch = Mathf.Clamp(value, 0f, 100f);
        soundControls.soundPitch = SFXPitch;
        SetSFXPitchInMixer(SFXPitch);
    }


    /*private void SetMasterPitchInMixer(float value)
    {
        audioMixer.SetFloat("masterPitch", value / 100f);
    }*/
    private void SetMusicPitchInMixer(float value)
    {
        audioMixer.SetFloat("musicPitch", value);
    }
    private void SetSFXPitchInMixer(float value)
    {
        audioMixer.SetFloat("sfxPitch", value);
    }
    #endregion
}