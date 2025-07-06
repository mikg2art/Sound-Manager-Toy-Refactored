using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundControls
{
    #region Variables
    private SoundLibrary soundLibrary;
    public float soundPitch;

    public SoundControls(SoundLibrary _soundLibrary, float _soundPitch)
    {
        soundLibrary = _soundLibrary;
        soundPitch = _soundPitch;
        SoundTypesAssignment();
    }

    private List<(AudioSource audioSource, float duration)> soundEvents = new List<(AudioSource audioSource, float duration)>();

    private SoundLibrary.ProtoAudioClip currentAudioClip = new SoundLibrary.ProtoAudioClip();

    private bool pauseAll = false;
    #endregion

    #region UpdateSoundEvents
    public void UpdateSoundEvents()
    {
        if (soundEvents.Count < 0) return;

        for (int i = soundEvents.Count - 1; i >= 0; i--)
        {
            if (soundEvents[i].audioSource.time >= soundEvents[i].duration)
            {
                ObjectPooler.i.ReturnToPool("SoundEffects", soundEvents[i].audioSource.gameObject);
                soundEvents.RemoveAt(i);
            }
        }
    }
    #endregion

    #region Create
    public void CreateSoundEvent(object type, int priority = 128, float pitch = -1, int volume = -1,
                                 float startTime = 0, float endTime = 0, float minDist = 0, float maxDist = 0, 
                                 Vector3 position = default(Vector3), bool loop = false)
    {
        SoundLibrary.ProtoAudioClip[] currentAudioClips = FindAudioClipsByType(type);

        FindAudioClip((int)type, currentAudioClips);

        CreateAudioSource(priority, pitch, volume, startTime, endTime, minDist, maxDist, position);
    }

    private SoundLibrary.ProtoAudioClip[] FindAudioClipsByType(object type)
    {
        switch (type)
        {
            case SoundEffect.PlayerActiveRageState:
                return soundLibrary.ActiveRageAudioClips;
            case SoundEffect.Background:
                return soundLibrary.BackgroundAudioClips;
            case SoundEffect.BigOniState:
                return soundLibrary.BigOniAudioClips;
            case SoundEffect.Environment:
                return soundLibrary.EnvironmentAudioClips;
            case SoundEffect.FinalBossState:
                return soundLibrary.FinalBossAudioClips;
            case SoundEffect.PlayerNoRageState:
                return soundLibrary.NoRageAudioClips;
            case SoundEffect.OniState:
                return soundLibrary.OniAudioClips;
            case SoundEffect.OrbState:
                return soundLibrary.OrbAudioClips;
            case SoundEffect.PlayerPassiveRageState:
                return soundLibrary.PassiveRageAudioClips;
            case SoundEffect.PlayerBaseState:
                return soundLibrary.UniversalAudioClips;
            case SoundEffect.YureiState:
                return soundLibrary.YureiAudioClips;
            default:
                Debug.LogWarning($"SoundEffect type '{type}' not recognized.");
                return null;
        }
    }

    private void FindAudioClip(int type, SoundLibrary.ProtoAudioClip[] protoAudioClips)
    {
        //If there are clips in the list
        if (protoAudioClips.Length <= 0)
            throw new Exception("Error. No sounds in the library.");

        List<SoundLibrary.ProtoAudioClip> randomAudioClips = new List<SoundLibrary.ProtoAudioClip>();

        //Find right audio clips from audio clip list
        foreach (SoundLibrary.ProtoAudioClip audioClip in protoAudioClips)
            if (audioClip.type == type)
                randomAudioClips.Add(audioClip);

        //If nothing found end function
        if (randomAudioClips.Count <= 0)
            throw new Exception("This sound type does not assigned in the library. Cannot play sound.");

        currentAudioClip = null;

        //If there is multiple clips assigned to the same type, choose random
        if (randomAudioClips.Count > 1)
            currentAudioClip = randomAudioClips[UnityEngine.Random.Range(0, randomAudioClips.Count)];
        else
            currentAudioClip = randomAudioClips[0];

        //Check if audio clip assigned
        if (currentAudioClip == null || currentAudioClip.audioClip == null)
            throw new ArgumentNullException(nameof(currentAudioClip.audioClip),
                "The AudioClip field in currentAudioClip is null. Cannot play sound.");
    }

    private void CreateAudioSource(int priority = 128, float pitch = -1, int volume = -1,
                                 float startTime = 0, float endTime = 0, float minDist = 0, float maxDist = 0,
                                 Vector3 position = default(Vector3))
    {
        AudioSource audioSource = ObjectPooler.i.SpawnFromPool("SoundEffects", position, Quaternion.identity).GetComponent<AudioSource>();

        audioSource.clip = currentAudioClip.audioClip;

        //Set volume
        if (volume >= 0 && volume <= 100)
            audioSource.volume = volume / 100;
        else 
            audioSource.volume = ((float)currentAudioClip.setup.volume / 100);

        //Set priority
        audioSource.priority = currentAudioClip.setup.priority;

        //Set pitch
        if (pitch >= 0 && pitch <= 3)
            audioSource.pitch = pitch + UnityEngine.Random.Range(-0.1f, 0.1f);
        else 
            audioSource.pitch = currentAudioClip.setup.pitch + UnityEngine.Random.Range(-0.1f, 0.1f);

        //If sound set to 3D add min and max distance.
        float min = currentAudioClip.setup.s3DSettings.minDistance;
        float max = currentAudioClip.setup.s3DSettings.maxDistance;

        if (minDist >= 0 && maxDist > 0)
        {
            min = minDist; max = maxDist;
        }

        if (min >= 0 && max > 0)
        {
            audioSource.spatialBlend = 1f;

            if (min < max)
                min = max - 1f;

            audioSource.minDistance = min;
            audioSource.maxDistance = max;
        }

        //Set time adjustments
        float clipLenght = audioSource.clip.length / soundPitch;

        if (endTime > 0 && endTime < clipLenght)
            clipLenght -= endTime / soundPitch;
        else if (currentAudioClip.setup.endTime > 0 && currentAudioClip.setup.endTime <= clipLenght)
            clipLenght -= currentAudioClip.setup.endTime / soundPitch;

        if (startTime > 0 && startTime < clipLenght)
            audioSource.time = startTime / soundPitch;
        else if (currentAudioClip.setup.startTime > 0 && currentAudioClip.setup.startTime < clipLenght)
            audioSource.time = currentAudioClip.setup.startTime / soundPitch;

        //Add to list and play
        soundEvents.Add((audioSource, clipLenght));
        audioSource.Play();
    }
    #endregion

    #region Commands
    public void PauseAllSFX()
    {
        if (pauseAll) return;
        if (soundEvents.Count < 0) return;
        
        pauseAll = true;
        for (int i = 0; i < soundEvents.Count; i++)
        {
            if (soundEvents[i].audioSource != null)
                soundEvents[i].audioSource.Pause();
        }        
    }

    public void PlayAllSFX()
    {
        if (!pauseAll) return;
        if (soundEvents.Count < 0) return;

        pauseAll = false;
        for (int i = 0; i < soundEvents.Count; i++)
        {
            if (soundEvents[i].audioSource != null)
                soundEvents[i].audioSource.Play();
        }
    }

    public void StopAllSFX()
    {
        if (soundEvents.Count < 0) return;

        pauseAll = true;
        for (int i = 0; i < soundEvents.Count; i++)
        {
            if (soundEvents[i].audioSource != null)
                soundEvents[i].audioSource.Stop();
        }        
    }
    #endregion

    #region Additional
    private void SoundTypesAssignment()
    {
        foreach (SoundLibrary.BackgroundAudioClip audioClip in soundLibrary.BackgroundAudioClips)
            audioClip.SetType((int)audioClip.soundType);

        foreach (SoundLibrary.EnvironmentAudioClip audioClip in soundLibrary.EnvironmentAudioClips)
            audioClip.SetType((int)audioClip.soundType);

        foreach (SoundLibrary.BaseAudioClip audioClip in soundLibrary.UniversalAudioClips)
            audioClip.SetType((int)audioClip.soundType);

        foreach (SoundLibrary.NoRageAudioClip audioClip in soundLibrary.NoRageAudioClips)
            audioClip.SetType((int)audioClip.soundType);

        foreach (SoundLibrary.PassiveRageAudioClip audioClip in soundLibrary.PassiveRageAudioClips)
            audioClip.SetType((int)audioClip.soundType);

        foreach (SoundLibrary.ActiveRageAudioClip audioClip in soundLibrary.ActiveRageAudioClips)
            audioClip.SetType((int)audioClip.soundType);

        foreach (SoundLibrary.OniAudioClip audioClip in soundLibrary.OniAudioClips)
            audioClip.SetType((int)audioClip.soundType);
        
        foreach (SoundLibrary.BigOniAudioClip audioClip in soundLibrary.BigOniAudioClips)
            audioClip.SetType((int)audioClip.soundType);

        foreach (SoundLibrary.YureiAudioClip audioClip in soundLibrary.YureiAudioClips)
            audioClip.SetType((int)audioClip.soundType);

        foreach (SoundLibrary.OrbAudioClip audioClip in soundLibrary.OrbAudioClips)
            audioClip.SetType((int)audioClip.soundType);

        foreach (SoundLibrary.FinalBossAudioClip audioClip in soundLibrary.FinalBossAudioClips)
            audioClip.SetType((int)audioClip.soundType);
    }
    #endregion
}