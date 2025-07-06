using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicControls
{
    #region Variables

    #region Constructor
    public MusicLibrary musicLibrary;
    public AudioSource musicEvent;
    public float musicPitch;

    public AudioSource previousTrack;
    public AudioSource currentTrack;
    public AudioSource nextTrack;

    private Func<MusicStates, MusicStates> GetNextMusicState;

    public MusicControls(MusicLibrary _musicLibrary, AudioSource _musicEvent, float _musicPitch, 
        AudioSource _trackObject1, AudioSource _trackObject2, Func<MusicStates, MusicStates> _getNextMusicState)
    {
        musicLibrary = _musicLibrary;
        musicEvent = _musicEvent;
        musicPitch = _musicPitch;

        previousTrack = _trackObject1;
        currentTrack = _trackObject2;
        nextTrack = null;

        GetNextMusicState = _getNextMusicState;

        SoundTypesAssignment();
    }

    #endregion

    #region Transition Variables
    private MusicStates currentMusicState;

    private float currentTrackEndTime;

    private float transitionTime;

    private float substractionValue; //for transition

    private float currentTrackVolume;
    private float previousTrackVolume;

    private bool musicStarted = false;
    private bool musicIsPlaying;
    private bool musicOnPause;

    private bool previousTrackIsOver;
    #endregion

    #endregion

    #region Update and Transition
    public void UpdateMusic()
    {
        if (!musicIsPlaying) return;

        CheckIfTrackIsOver();

        //works only when track is over and transition time > 0
        Transition();
    }
    private void CheckIfTrackIsOver() 
    {
        if (!previousTrackIsOver && currentTrack.time >= currentTrackEndTime - transitionTime)
        {
            CreateMusicEvent(GetNextMusicState(currentMusicState));
            if (transitionTime != 0)
            {
                previousTrackIsOver = true; //needs for smooth transition
                currentTrack.volume = 0;
            }
            else previousTrack.volume = 0;
        }
    }
    public void ChangeTrack(MusicStates nextState)
    {
        CreateMusicEvent(nextState);

        if (!musicStarted)
        {
            musicStarted = true;
            musicIsPlaying = true;
            musicOnPause = false;
            return;
        }

        if (previousTrackIsOver) return;

        if (transitionTime == 0)
        {
            previousTrack.volume = 0;
        }
        else
        {
            previousTrackIsOver = true; //needs for smooth transition
            currentTrack.volume = 0;
        }
    }
    private void Transition()
    {
        if (!previousTrackIsOver) return;

        substractionValue += 0.0085f / transitionTime;

        if (previousTrack.volume > 0)
            previousTrack.volume = Mathf.Lerp(previousTrackVolume, 0, substractionValue);
        else
            previousTrack.volume = 0;

        if (currentTrack.volume < currentTrackVolume)
            currentTrack.volume = Mathf.Lerp(0, currentTrackVolume, substractionValue);
        else
            currentTrack.volume = currentTrackVolume;

        if (previousTrack.volume <= 0 && currentTrack.volume >= currentTrackVolume)
        {
            substractionValue = 0;
            previousTrackIsOver = false;
        }        
    }
    #endregion

    #region Commands
    public void PlayMusic() 
    {
        //PlayMusic only purpose is to unpause music
        if (!musicOnPause) return;

        musicIsPlaying = true;
        musicOnPause = false;
        if (previousTrack != null) previousTrack.Play();
        if (currentTrack != null)  currentTrack.Play();
    }
    public void PauseMusic() 
    {
        if (!musicIsPlaying || musicOnPause) return;

        musicIsPlaying = false;
        musicOnPause = true;
        if (previousTrack != null) previousTrack.Pause();
        if (currentTrack != null)  currentTrack.Pause();
    }
    public void StopMusic()
    {
        musicIsPlaying = false;
        musicOnPause = false;
        musicStarted = false;
        if (previousTrack != null)  previousTrack.Stop();
        if (currentTrack != null)   currentTrack.Stop();
    }
    #endregion

    #region Set & Get Functions
    public bool IsMusicPlaying() => musicIsPlaying;
    public bool IsMusicPlaying(bool value) 
    {
        if (!value)
        {
            //If transition in process
            substractionValue = 0;
            previousTrackIsOver = false;
        }

        return musicIsPlaying = value;
    }
    public float SetTransitionTime(float newTime) => transitionTime = newTime;
    public MusicStates GetMusicState() => currentMusicState;
    public MusicStates SetMusicState(MusicStates newState) => currentMusicState = newState;
    #endregion

    #region Create
    private void CreateMusicEvent(MusicStates type)
    {
        if (musicPitch <= 0) throw new Exception("Error. Music pitch is 0 or less.");

        MusicLibrary.ProtoMusicClip currentMusicClip = FindAudioClip(type);

        currentMusicState = type;

        if (previousTrackIsOver) {
            //If track is about to change in the middle of transition
            SetupTrack(ref currentTrack, currentMusicClip, true);
            currentTrack.Play();
            return;
        }

        nextTrack = previousTrack;
        previousTrack = currentTrack;

        SetupTrack(ref nextTrack, currentMusicClip);

        currentTrack = nextTrack;

        previousTrackVolume = previousTrack.volume;
        currentTrackVolume = currentTrack.volume;

        nextTrack = null;

        currentTrack.Play();
    }
    private MusicLibrary.ProtoMusicClip FindAudioClip(MusicStates type)
    {
        MusicLibrary.MusicSampleClip[] musicClips = musicLibrary.MusicSampleClips;
        if (musicClips.Length <= 0) throw new Exception("Error. No effects in the library.");

        List<MusicLibrary.ProtoMusicClip> possibleMusicClips = new List<MusicLibrary.ProtoMusicClip>();
        foreach (MusicLibrary.MusicSampleClip musicClip in musicClips)
        {
            if (musicClip.musicSampleType == type)
            {
                possibleMusicClips.Add(musicClip);
            }
        }

        if (possibleMusicClips.Count <= 0) throw new Exception("Error. This effect type does not assigned in the library.");

        MusicLibrary.ProtoMusicClip rightMusicClip;
        if (possibleMusicClips.Count > 1)
            rightMusicClip = possibleMusicClips[UnityEngine.Random.Range(0, possibleMusicClips.Count)];
        else
            rightMusicClip = possibleMusicClips[0];

        if (rightMusicClip == null || rightMusicClip.audioClip == null)
            throw new ArgumentNullException(nameof(rightMusicClip.audioClip),
                "The MusicClip field in currentMusicClip is null. Cannot play sound.");

        return rightMusicClip;
    }
    private void SetupTrack(ref AudioSource track, MusicLibrary.ProtoMusicClip currentMusicClip, bool swap = false)
    {
        track.clip = currentMusicClip.audioClip;

        if (swap)
        {
            currentTrackVolume = (float)currentMusicClip.setup.volume / 100;
            if (track.volume > currentTrackVolume)
                track.volume = currentTrackVolume;
        }
        else track.volume = (float)currentMusicClip.setup.volume / 100;

        track.priority = currentMusicClip.setup.priority;
        track.pitch = currentMusicClip.setup.pitch;

        float clipLenght = track.clip.length / musicPitch;
        float startTime = Mathf.Abs(currentMusicClip.setup.startTime) / musicPitch;
        float endTime = Mathf.Abs(currentMusicClip.setup.endTime) / musicPitch;
        transitionTime = Mathf.Abs(currentMusicClip.transitionTime) / musicPitch;

        if (endTime > 0 && endTime <= clipLenght)
            clipLenght -= endTime;

        if (startTime > 0 && startTime < clipLenght)
            track.time = startTime;

        currentTrackEndTime = clipLenght;
    }
    #endregion

    #region Additional
    private void SoundTypesAssignment()
    {
        foreach (MusicLibrary.MusicSampleClip musicSampleClip in musicLibrary.MusicSampleClips)
            musicSampleClip.SetType((int)musicSampleClip.musicSampleType);
    }
    #endregion
}