using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonsFirstScene : Buttons
{
    private Timer reviveTimer = new Timer();
    private bool playerIsDead = false;
    private MusicStates requestedState;

    [Header ("Revive Time")]
    public float reviveTime = 3;

    protected override void Start()
    {
        timers.Add(reviveTimer);
    }

    protected override void Update()
    {
        base.Update();

        if (playerIsDead && reviveTimer.IsCompleted())
        {
            playerIsDead = false;

            MusicControls musicControls = SoundManager.i.musicControls;

            SoundManager.i.musicControls.IsMusicPlaying(true);

            if (!musicStopped)
            {
                musicControls.ChangeTrack(requestedState);
                musicControls.SetTransitionTime(reviveTime);
                musicControls.previousTrack.volume = 0;
            }
        }

        if (playerIsInTheEnd) {
            if (SoundManager.i.musicControls.GetMusicState() == MusicStates.MusicLevel5Final)
            {
                musicTitleText.text = "Game Over, Level 5";
                marker.position = musicButtons[8].position;
            }
        }
    }

    //Sound Effects
    public override void PlaySoundEffect(int num)
    {
        base.PlaySoundEffect(num);
        
        switch (num)
        {
            case 1:
                SoundManager.i.soundControls.CreateSoundEvent(SoundEffect.PlayerActiveRageState.FirstAttack);
                break;
            case 2:
                SoundManager.i.soundControls.CreateSoundEvent(SoundEffect.BigOniState.TalkTwo);
                break;
            case 3:
                SoundManager.i.soundControls.CreateSoundEvent(SoundEffect.PlayerNoRageState.Slash);
                break;
            case 4:
                SoundManager.i.soundControls.CreateSoundEvent(SoundEffect.PlayerActiveRageState.RageActivate);
                break;
            case 5:
                SoundManager.i.soundControls.CreateSoundEvent(SoundEffect.OrbState.Destroyed);
                break;

            default:
                break;
        }
        
    }

    #region Control Buttons
    public void Play()
    {
        pauseButtonPressed = false;
        musicStopped = false;
        SoundManager.i.soundControls.PlayAllSFX();
        SoundManager.i.musicControls.PlayMusic();
        highlightPPB.position = playButton.position;
        Time.timeScale = 1;
    }
    public void Pause()
    {
        pauseButtonPressed = true;
        SoundManager.i.soundControls.PauseAllSFX();
        SoundManager.i.musicControls.PauseMusic();
        highlightPPB.position = pauseButton.position;
        Time.timeScale = 0;
    }
    public void Stop()
    {
        musicStopped = true;
        reviveTimer.SetMaxTime(0);
        SoundManager.i.soundControls.StopAllSFX();
        SoundManager.i.musicControls.StopMusic();
    }
    #endregion



    public override void PlayMusic(int num)
    {
        base.PlayMusic(num);

        musicStopped = false;

        requestedState = MusicStates.MusicMenu;

        switch (num)
        {
            case 1:
                requestedState = MusicStates.MusicLevel0Start;
                musicTitleText.text = "Music Start, Level 0";
                break;
            case 2:
                requestedState = MusicStates.MusicLevel1Continue;
                musicTitleText.text = "Exploring, Level 1";
                break;                
            case 3:
                requestedState = MusicStates.MusicLevel2Start;
                musicTitleText.text = "Battle, Level 2";
                break;
            case 4:
                requestedState = MusicStates.MusicLevel2End;
                musicTitleText.text = "Boss 1 Defeated, Level 2";
                break;
            case 5:
                requestedState = MusicStates.MusicLevel3Start;
                musicTitleText.text = "Going To Fin. Boss, Level 3";
                break;
            case 6:
                requestedState = MusicStates.MusicLevel4Part1;
                musicTitleText.text = "Boss 2 First Stage, Level 4 Part 1";
                break;
            case 7:
                requestedState = MusicStates.MusicLevel4Part2;
                musicTitleText.text = "Boss 2 Second Stage, Level 4 Part 2";
                break;
            case 8:
                requestedState = MusicStates.MusicLevel4End;
                musicTitleText.text = "Boss 2 Defeated, Level 4 part 3";
                break;
            case 9:
                requestedState = MusicStates.MusicLevel5Final;
                musicTitleText.text = "Game Over, Level 5";
                break;

            default:
                break;
        }

        if(!playerIsDead)
            SoundManager.i.musicControls.ChangeTrack(requestedState);
    }



    public void PlayerIsDead()
    {
        if (musicStopped || pauseButtonPressed || musicButtons.Count <= 0) return;

        playerIsDead = true;

        reviveTimer.SetMaxTime(reviveTime);

        int i = playerIsInTheEnd ? 4 : 1;
        marker.position = musicButtons[i].position;
        musicTitleText.text = "Player Dead, Level " + (i + 1);

        SoundManager.i.soundControls.CreateSoundEvent(SoundEffect.PlayerBaseState.Death);
        SoundManager.i.musicControls.currentTrack.volume = 0;
        SoundManager.i.musicControls.IsMusicPlaying(false);

        if (playerIsInTheEnd)
            requestedState = MusicStates.MusicLevel3Start;
        else
            requestedState = MusicStates.MusicLevel2Part2;
    }
}
