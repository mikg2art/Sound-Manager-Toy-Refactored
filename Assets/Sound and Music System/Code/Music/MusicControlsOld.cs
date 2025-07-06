using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicControlsOld : MonoBehaviour { 
    /*[SerializeField] public MusicLibrary musicLibrary;//Change later!
    [SerializeField] public AudioSource musicEvent;//Change later!
    [SerializeField] [Range(0f, 10f)] public float musicPitch = 1;//Change later!

    public class CMusicEvent
    {
        public int type;
        public AudioSource mEvent;
    }

    //MusicSystem
    private List<CMusicEvent> musicEvents;

    private MusicLibrary.ProtoMusicClip currentPMC; //PMC - Proto Music Clip
    //private MusicLibrary.ProtoMusicClip prevPMC;
    [HideInInspector] public AudioSource currentAS; //AS - Audio Source
    [HideInInspector] public AudioSource prevAS;

    [HideInInspector] public Timer startTransitionTimer;
    //[HideInInspector] public Timer endTransitionTimer;

    [HideInInspector] public float endTransiTime;
    [HideInInspector] public bool l1endCase;
    [HideInInspector] public bool volumeChange;
    [HideInInspector] public bool musicIsPlaying;
    [HideInInspector] public bool playerIsDead;
    [HideInInspector] public bool playTransitionEffect;
    [HideInInspector] public bool zeroTransition;
    [HideInInspector] public bool theEnd;
    //[HideInInspector] public bool musicReset;

    public bool oldLibrarySolution;

    public enum battleLevels { L1, L2, L3 }
    [HideInInspector] public battleLevels battleLevel;

    public enum battleStates { playerRevived, battleOver, battleStarted }
    [HideInInspector] public battleStates battleState;

    [Header("Time to music change after battle ends:")]
    [SerializeField] public float battleOverTime = 10;

    [HideInInspector] public Timer deathTimer;
    [HideInInspector] public Timer battleTimer;
    [HideInInspector] public Timer effectTimer;
    [HideInInspector] public float percentage1;
    [HideInInspector] public float percentage2;
    [HideInInspector] public float currentVolume;
    [HideInInspector] public float prevVolume;
    public enum musicStates { none, menu, started, level1, level1End, level2, level2End, level3Start, level2l2, level3, level3End, level4s1, level4s2, level4End, level5 }//new
    [HideInInspector] public musicStates musicState;

    private void Awake()
    {
        //MusicSystem
        musicEvents = new List<CMusicEvent>();

        deathTimer = new Timer();
        battleTimer = new Timer();
        effectTimer = new Timer();

        percentage1 = 0;
        percentage2 = 0;
        musicState = musicStates.none;
        battleState = battleStates.playerRevived;
        volumeChange = false;
        musicIsPlaying = false;
        playerIsDead = false;
        playTransitionEffect = false;
        zeroTransition = false;
        theEnd = false;
        //musicReset = false;
        battleTimer.SetMaxTime(battleOverTime);

        l1endCase = true;

        //MusicSystem
        currentPMC = new MusicLibrary.ProtoMusicClip();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region MusicSystem

    public void L1endCase(bool b)
    {
        l1endCase = b;
    }


    public void MusicStart(bool again = false)
    {
        MusicStop();
        if (currentAS != null && currentAS.loop == true) { Destroy(currentAS.gameObject); }
        currentAS = null; prevAS = null;
        musicEvents.Clear();
        musicIsPlaying = true;

        /*
            For this showcase I decided to use old music solution.
            Feels weird to share music that wasn't ever licensed.
            It just was created by our musician for this project
            So I'm not sure if I can share it on my website.
        

        if (oldLibrarySolution)
        {
            musicState = musicStates.started;
            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML0Start), musicLibrary.MusicSampleClips);
        }
        else
        {
            musicState = musicStates.menu;
            if (!again)
                CreateMusicEvent((int)(MusicTypes.MusicSamples.Menu), musicLibrary.MusicSampleClips);
            else
                CreateMusicEvent((int)(MusicTypes.MusicSamples.Menu1), musicLibrary.MusicSampleClips);
        }
    }

    public void MusicRestart()
    {
        if (musicIsPlaying)
        {
            endTransiTime = currentPMC.setup.endTime / musicPitch;
            volumeChange = true;

            musicState = musicStates.started;
            if (currentPMC.type == (int)MusicTypes.MusicSamples.ML0p1)
            {
                CreateMusicEvent((int)(MusicTypes.MusicSamples.ML0p2), musicLibrary.MusicSampleClips, _volume: 0);
            }
            else CreateMusicEvent((int)(MusicTypes.MusicSamples.ML0p1), musicLibrary.MusicSampleClips, _volume: 0);
        }
    }

    private void MusicCycleNextSample()
    {
        if (!musicIsPlaying || playerIsDead) return;

        if (currentPMC == null) return;
        if (currentPMC.setup.endTime < 0) return;

        if (currentAS == null) throw new Exception("curAS = null");

        if (currentAS.time < currentAS.clip.length - currentPMC.setup.endTime) return;

        endTransiTime = 0.1f / musicPitch;

        volumeChange = true;
        switch (musicState)
        {
            case musicStates.menu:
                if (currentPMC.type == (int)MusicTypes.MusicSamples.Menu ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML5Fin ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML4End ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.Menu1)
                {
                    endTransiTime = 1f / musicPitch;
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.Menu2), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.Menu2)
                {
                    endTransiTime = 1f / musicPitch;
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.Menu1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                break;

            case musicStates.started:
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML0Start ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML0p2)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML0p1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML0p1)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML0p2), musicLibrary.MusicSampleClips, _volume: 0);
                }
                break;

            case musicStates.level1:
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML1p1 ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML5Fin)
                {
                    endTransiTime = 1f / musicPitch;
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1p2), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML1p2)
                {
                    endTransiTime = 1f / musicPitch;
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1p1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                break;

            case musicStates.level1End:

                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML1End_p1)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1End_p2), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML1End_p2)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1End_p1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                break;

            case musicStates.level2:
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML2p1 ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML2Start ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML2Death1 ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML2Death2)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML2p2), musicLibrary.MusicSampleClips, _volume: 0);
                    if (playTransitionEffect) effectTimer.SetMaxTime((currentAS.clip.length - 2.5f) / musicPitch);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML2p2)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML2p1), musicLibrary.MusicSampleClips, _volume: 0);
                    if (playTransitionEffect) effectTimer.SetMaxTime((currentAS.clip.length - 2.5f) / musicPitch);
                }
                break;

            case musicStates.level2End:
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML2End)
                {
                    MusicChangeLevel(musicStates.level3Start);
                }
                break;

            case musicStates.level3Start://for new library
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3bS1)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3bS2), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3bS2)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3bS1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                break;

            case musicStates.level2l2://for new library
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3e1)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3e2), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3e2)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3e1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                break;

            case musicStates.level3: //and new and old
                if (oldLibrarySolution)
                {
                    if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3Start ||
                        currentPMC.type == (int)MusicTypes.MusicSamples.ML3p2 ||
                        currentPMC.type == (int)MusicTypes.MusicSamples.ML3Death1 ||
                        currentPMC.type == (int)MusicTypes.MusicSamples.ML3Death2)
                    {
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3p1), musicLibrary.MusicSampleClips, _volume: 0);
                    }
                    else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3p1)
                    {
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3p2), musicLibrary.MusicSampleClips, _volume: 0);
                    }
                }
                else
                {
                    if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3b1 ||
                        currentPMC.type == (int)MusicTypes.MusicSamples.ML3bS1 ||
                        currentPMC.type == (int)MusicTypes.MusicSamples.ML3bS2 ||
                        currentPMC.type == (int)MusicTypes.MusicSamples.ML3bD1 ||
                        currentPMC.type == (int)MusicTypes.MusicSamples.ML3bD2)
                    {
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3b2), musicLibrary.MusicSampleClips, _volume: 0);
                    }
                    else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3b2)
                    {
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3b1), musicLibrary.MusicSampleClips, _volume: 0);
                    }
                }
                break;

            case musicStates.level3End:
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3Start ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML3Death1 ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML3Death2 ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML3p2)
                {
                    endTransiTime = currentPMC.setup.endTime / musicPitch; zeroTransition = false;
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3p1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3p1)
                {
                    endTransiTime = currentPMC.setup.endTime / musicPitch; zeroTransition = false;
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3p2), musicLibrary.MusicSampleClips, _volume: 0);
                }
                break;

            case musicStates.level4s1:
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML4Start ||
                    currentPMC.type == (int)MusicTypes.MusicSamples.ML4p1_2)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML4p1_1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML4p1_1)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML4p1_2), musicLibrary.MusicSampleClips, _volume: 0);
                }
                break;

            case musicStates.level4s2:
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML4p2_2)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML4p2_1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML4p2_1)
                {
                    CreateMusicEvent((int)(MusicTypes.MusicSamples.ML4p2_2), musicLibrary.MusicSampleClips, _volume: 0);
                }
                break;

            case musicStates.level4End:
                if (currentPMC.type == (int)MusicTypes.MusicSamples.ML4End)
                {
                    if (oldLibrarySolution)
                        MusicChangeLevel(musicStates.level5);
                    else
                        MusicChangeLevel(musicStates.menu);
                }
                break;

            case musicStates.level5:

                break;
        }

    }

    private void MusicCycleUpdateSample()
    {
        if (musicIsPlaying)
        {
            if (volumeChange)
            {
                if (currentAS != null)
                {
                    if (prevAS != null)
                    {
                        if (!zeroTransition)
                        {
                            if (!playerIsDead)
                            {
                                if (currentAS.volume < currentVolume)
                                {
                                    //Debug.Log("VolumeC: " + currentAS.volume);
                                    currentAS.volume = Mathf.Lerp(0, currentVolume, percentage1);
                                    percentage1 += 0.0085f / endTransiTime;
                                }
                            }

                            if (prevAS.volume > 0)
                            {
                                //Debug.Log("VolumeP: " + prevAS.volume);
                                prevAS.volume = Mathf.Lerp(prevVolume, 0, percentage2);
                                percentage2 += 0.0085f / endTransiTime;
                            }

                            if (prevAS.volume <= 0 && currentAS.volume >= currentVolume)
                            {
                                //Debug.Log("true-true");
                                percentage1 = 0;
                                percentage2 = 0;
                                prevAS.volume = 0;
                                currentAS.volume = currentVolume;
                                volumeChange = false;
                            }
                        }
                        else
                        {
                            //Debug.Log("ZeroT");
                            percentage1 = 0;
                            percentage2 = 0;
                            currentAS.volume = currentVolume;
                            prevAS.volume = 0;
                            volumeChange = false;
                            zeroTransition = false;
                        }
                    }
                    else Debug.Log("prevAC = null");
                }
                else Debug.Log("curAC = null");
            }

        }
    }

    public void MusicChangeLevel(musicStates nextState)
    {
        if (musicIsPlaying)
        {
            if (musicState != nextState)
            {
                if (currentAS != null)
                {
                    //if (currentPMC.setup.endTime != 0)
                    endTransiTime = currentPMC.setup.endTime / musicPitch;
                    //else 
                    //endTransiTime = 0;

                    volumeChange = true;
                    switch (nextState)
                    {
                        case musicStates.menu:
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.Menu), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.started:
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML0p1), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level1:
                            if (musicState == musicStates.level2)
                                endTransiTime = currentPMC.setup.endTime + 2f / musicPitch;
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1p1), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level1End:
                            l1endCase = false;
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1End_p1), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level2:
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML2Start), musicLibrary.MusicSampleClips, _volume: 0);
                            playTransitionEffect = true;
                            effectTimer.SetMaxTime((currentAS.clip.length - 1.9f) / musicPitch);
                            break;
                        case musicStates.level2End:
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML2End), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level3Start:
                            if (oldLibrarySolution)
                                CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3Start), musicLibrary.MusicSampleClips, _volume: 0);
                            else
                                CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3bS1), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level2l2:
                            if (musicState == musicStates.level3)
                                endTransiTime = currentPMC.setup.endTime + 2f / musicPitch;
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3e1), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level3:
                            if (oldLibrarySolution)
                                CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3Start), musicLibrary.MusicSampleClips, _volume: 0);
                            else
                                CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3bS1), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level3End:
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3Start), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level4s1:
                            endTransiTime = 0.1f;
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML4Start), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level4s2:
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML4p2_1), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level4End:
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML4End), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                        case musicStates.level5:
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML5Fin), musicLibrary.MusicSampleClips, _volume: 0);
                            break;
                    }
                    musicState = nextState;
                }
                else Debug.Log("curAC = null");
            }
            //else Debug.Log("StateIsTheSame");
        }
    }

    public void SetMusicBattleState(battleStates nextBattleState)
    {
        if (musicIsPlaying)
        {
            switch (nextBattleState)
            {
                case battleStates.battleOver:
                    if (battleLevel == battleLevels.L2)
                        battleTimer.SetMaxTime((battleOverTime + 60f) / musicPitch);
                    else
                        battleTimer.SetMaxTime(battleOverTime / musicPitch);
                    break;

                case battleStates.battleStarted:
                    battleTimer.TimerReset();
                    if (battleLevel == battleLevels.L1)
                        MusicChangeLevel(musicStates.level1End);
                    if (battleLevel == battleLevels.L2)
                        MusicChangeLevel(musicStates.level2);
                    if (battleLevel == battleLevels.L3)
                        MusicChangeLevel(musicStates.level3);
                    break;

                case battleStates.playerRevived:
                    battleTimer.TimerReset();
                    break;
            }
            battleState = nextBattleState;
        }
    }

    public void MusicPlayerIsDead(bool closeToArena = false, bool loadCheck = false)
    {
        if (musicIsPlaying)
        {
            playerIsDead = true;
            SetMusicBattleState(battleStates.playerRevived);
            if (battleTimer != null)
                battleTimer.TimerReset();
            if (!loadCheck)
                deathTimer.SetMaxTime(4.0f / musicPitch);
            else
                deathTimer.SetMaxTime(0.5f / musicPitch);
            endTransiTime = 2f / musicPitch;
            volumeChange = true;
            if (closeToArena)
            {
                if (battleLevel == battleLevels.L1)
                {
                    musicState = musicStates.started;
                    if (currentPMC.type == (int)MusicTypes.MusicSamples.ML1Death2)
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML2Death1), musicLibrary.MusicSampleClips, _volume: 0);
                    else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML2Death1)
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1Death2), musicLibrary.MusicSampleClips, _volume: 0);
                    else
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1Death1), musicLibrary.MusicSampleClips, _volume: 0);
                }

                if (battleLevel == battleLevels.L2)
                {
                    musicState = musicStates.level2;
                    if (currentPMC.type == (int)MusicTypes.MusicSamples.ML2Death2)
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML2Death1), musicLibrary.MusicSampleClips, _volume: 0);
                    else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML2Death1)
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML2Death2), musicLibrary.MusicSampleClips, _volume: 0);
                    else
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML2Death1), musicLibrary.MusicSampleClips, _volume: 0);
                }

                if (battleLevel == battleLevels.L3)
                {
                    musicState = musicStates.level3;
                    if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3bD1)
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3bD2), musicLibrary.MusicSampleClips, _volume: 0);
                    else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3bD1)
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3bD2), musicLibrary.MusicSampleClips, _volume: 0);
                    else
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3bD1), musicLibrary.MusicSampleClips, _volume: 0);

                }

                playTransitionEffect = true;
                effectTimer.SetMaxTime((currentAS.clip.length - 1.9f) / musicPitch);
            }
            else
            {
                if (musicState == musicStates.level3End || musicState == musicStates.level4s1 ||
                    musicState == musicStates.level4s2 || musicState == musicStates.level4End)
                {
                    musicState = musicStates.level3End;
                    if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3Death2)
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3Death1), musicLibrary.MusicSampleClips, _volume: 0);
                    else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3Death1)
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3Death2), musicLibrary.MusicSampleClips, _volume: 0);
                    else
                        CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3Death1), musicLibrary.MusicSampleClips, _volume: 0);
                }
                else
                {
                    if (battleLevel == battleLevels.L1)
                    {
                        musicState = musicStates.started;
                        if (currentPMC.type == (int)MusicTypes.MusicSamples.ML0p2)
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML0p1), musicLibrary.MusicSampleClips, _volume: 0);
                        else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML0p1)
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML0p2), musicLibrary.MusicSampleClips, _volume: 0);
                        else
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML0p1), musicLibrary.MusicSampleClips, _volume: 0);
                    }
                    if (battleLevel == battleLevels.L2)
                    {
                        musicState = musicStates.level1;
                        if (currentPMC.type == (int)MusicTypes.MusicSamples.ML1p2)
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1p1), musicLibrary.MusicSampleClips, _volume: 0);
                        else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML1p1)
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1p2), musicLibrary.MusicSampleClips, _volume: 0);
                        else
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML1p1), musicLibrary.MusicSampleClips, _volume: 0);
                    }
                    if (battleLevel == battleLevels.L3)
                    {
                        musicState = musicStates.level2l2;
                        if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3e2)
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3e1), musicLibrary.MusicSampleClips, _volume: 0);
                        else if (currentPMC.type == (int)MusicTypes.MusicSamples.ML3e1)
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3e2), musicLibrary.MusicSampleClips, _volume: 0);
                        else
                            CreateMusicEvent((int)(MusicTypes.MusicSamples.ML3e1), musicLibrary.MusicSampleClips, _volume: 0);
                    }
                }
            }
        }
    }

    #region Play/Pause/Stop
    public void MusicPause(bool lp = false)
    {
        if (lp)
            SetMusicLowPass(4000f);
        else
        {
            if (musicEvents.Count > 0)
            {
                musicIsPlaying = false;
                for (int i = 0; i < musicEvents.Count; i++)
                {
                    if (musicEvents[i].mEvent != null)
                        musicEvents[i].mEvent.Pause();
                }
            }
        }
    }

    public void MusicPlay(bool lp = false)
    {
        if (lp)
            SetMusicLowPass(22000f);
        else
        {
            musicIsPlaying = true;
            if (musicEvents.Count > 0)
            {
                for (int i = 0; i < musicEvents.Count; i++)
                {
                    if (musicEvents[i].mEvent != null)
                        musicEvents[i].mEvent.Play();
                }
            }
        }
    }

    public void MusicStop()
    {
        musicIsPlaying = false;
        if (musicEvents.Count > 0)
        {
            for (int i = 0; i < musicEvents.Count; i++)
            {
                if (musicEvents[i].mEvent != null)
                    musicEvents[i].mEvent.Stop();
            }
        }
    }
    #endregion

    private void CreateMusicEvent(int _type, MusicLibrary.ProtoMusicClip[] arr, float _volume = 1, float _startTime = 0, float _endTime = 0, bool effect = false, bool loop = false)
    {
        if (effect)
        {
            if (arr.Length >= 0)
            {
                List<MusicLibrary.ProtoMusicClip> seqPMC = new List<MusicLibrary.ProtoMusicClip>();
                MusicLibrary.ProtoMusicClip pmcHelp = new MusicLibrary.ProtoMusicClip();
                foreach (MusicLibrary.ProtoMusicClip musicClip in arr)
                {
                    if (musicClip.type == _type)
                    {
                        seqPMC.Add(musicClip);
                    }
                }
                if (seqPMC.Count > 0)
                {
                    if (seqPMC.Count > 1)
                    {
                        int randomNum = Random.Range(0, seqPMC.Count);
                        pmcHelp = seqPMC[randomNum];
                    }
                    else
                    {
                        pmcHelp = seqPMC[0];
                    }
                    if (pmcHelp.audioClip != null)
                    {
                        AudioSource audioSource = new AudioSource();

                        audioSource = Instantiate(musicEvent, Vector3.zero, Quaternion.identity);

                        audioSource.clip = pmcHelp.audioClip;

                        float curVol = (float)pmcHelp.setup.volume / 100;
                        audioSource.volume = curVol;

                        audioSource.priority = pmcHelp.setup.priority;

                        audioSource.pitch = pmcHelp.setup.pitch;

                        CMusicEvent cMusicEvent = new CMusicEvent();
                        cMusicEvent.mEvent = audioSource;
                        cMusicEvent.type = _type;

                        bool help = true;
                        if (musicEvents.Count > 0)
                        {
                            for (int i = 0; i < musicEvents.Count; i++)
                            {
                                if (musicEvents[i].type == cMusicEvent.type)
                                {
                                    if (musicEvents[i].mEvent != null)
                                    {
                                        GameObject help1 = musicEvents[i].mEvent.gameObject;
                                        musicEvents[i] = null; //а точно так надо?
                                        Destroy(help1);
                                    }

                                    musicEvents[i] = cMusicEvent;
                                    help = false;
                                }
                            }
                        }

                        if (help)
                            musicEvents.Add(cMusicEvent);

                        float clipLenght = audioSource.clip.length / musicPitch;

                        audioSource.Play();

                        Destroy(audioSource.gameObject, clipLenght);
                    }
                }
                else Debug.Log("Error. This effect type does not assigned in the library.");
            }
            else Debug.Log("Error. No effects in the library.");
        }
        else
        {
            if (arr.Length >= 0)
            {//Если в массиве что-то вобще есть
                //prevPMC = currentPMC;
                currentPMC = null;
                //currentAudioClip = null;
                List<MusicLibrary.ProtoMusicClip> seqPMC = new List<MusicLibrary.ProtoMusicClip>();

                foreach (MusicLibrary.ProtoMusicClip musicClip in arr)
                {//То найти подходящий звук из массива.
                 //Debug.Log("pAudioClip.type = " + uAudioClip.type + " , type = " + type);
                    if (musicClip.type == _type)
                    {
                        seqPMC.Add(musicClip);
                    }
                }
                if (seqPMC.Count > 0)
                {//Если звук нашелся.
                    if (seqPMC.Count > 1)
                    {//Если больше одного на тип то выбираем по порядку.
                     //пока не уверен как, можно в будущем дописать.
                        int randomNum = Random.Range(0, seqPMC.Count);
                        currentPMC = seqPMC[randomNum];
                    }
                    else
                    {//Если нет то первый.
                        currentPMC = seqPMC[0];
                    }

                    if (currentPMC.audioClip != null)
                    {
                        AudioSource audioSource = new AudioSource();

                        audioSource = Instantiate(musicEvent, Vector3.zero, Quaternion.identity);
                        prevAS = currentAS;
                        currentAS = audioSource;

                        audioSource.clip = currentPMC.audioClip;

                        prevVolume = currentVolume;
                        currentVolume = (float)currentPMC.setup.volume / 100;
                        audioSource.volume = currentVolume;
                        if (_volume == 0) audioSource.volume = 0;

                        audioSource.priority = currentPMC.setup.priority;

                        audioSource.pitch = currentPMC.setup.pitch;

                        audioSource.loop = loop;//?

                        CMusicEvent cMusicEvent = new CMusicEvent();
                        cMusicEvent.mEvent = audioSource;
                        cMusicEvent.type = _type;

                        bool help = true;
                        if (musicEvents.Count > 0)
                        {
                            for (int i = 0; i < musicEvents.Count; i++)
                            {
                                if (musicEvents[i].type == cMusicEvent.type)
                                {
                                    if (musicEvents[i].mEvent != null)
                                    {
                                        GameObject help1 = musicEvents[i].mEvent.gameObject;
                                        musicEvents[i] = null; //а точно так надо?
                                        Destroy(help1);
                                    }

                                    musicEvents[i] = cMusicEvent;
                                    help = false;
                                }
                            }
                        }

                        if (help)
                            musicEvents.Add(cMusicEvent);

                        float clipLenght = audioSource.clip.length / musicPitch;

                        if (currentPMC.setup.startTime > 0 && currentPMC.setup.startTime < clipLenght)
                        {
                            audioSource.time = currentPMC.setup.startTime;
                        }

                        audioSource.Play();

                        if (!loop)
                            Destroy(audioSource.gameObject, clipLenght + 5f);
                    }
                }
                else Debug.Log("Error. This music type does not assigned in the library.");
            }
            else Debug.Log("Error. No music in the library.");
        }
    }

    /*
    Методы муз системы:
    1. Начать музыку - уровень 0. +
    2. Зациклить на уровне 1 - игрок в меню. +
    3. Когда начнется игра перейти в уровень 2 - игрок навчал игру и возможно стоит тупит. +
    4. Когда игрок пошел в бой с какого-то момента перейти в динамику (уровень 2.5) и потом в бой - уровень 3 +
    5. Иметь возможность вернуться назад на уровень 2 и возможно 1. +
    6. Зациклить на уровне 3 на время и если игрок не сражается то вернуться на уровень 2 (но если бой близко то ненадо мб)
    7. При смерти все отключать и включать заного после секундной паузы.+
    8. При окончании первой локации завершать трек естественно на его вершине (уровень 3.5) и перейти на начало 2-ки. +
    9. При переключении между музыкой создавать вуу и жжж эффекты чтобы скрыть неидеальность (мозможно нескольких типов). +
    10. При боссе запускать особый трек перехода на уровень 4 и в конце запускать уровень 5 - конец + подумать еще над этим. +
    11. Иметь таймер после которого музыка уйдет на уровень вниз вне боя. +
    12. Добавить звук смерти игрока который мне понравился.
    13. Сделать чтобы музыка перед поссом и музыка босса колайдились или тпа того.
    
    #endregion


    private void SoundTypesAssignment()
    {
        //SFX
        foreach (SoundLibrary.BackgroundAudioClip uac in soundLibrary.BackgroundAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.EnvironmentAudioClip uac in soundLibrary.EnvironmentAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.UniversalAudioClip uac in soundLibrary.UniversalAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.NoRageAudioClip uac in soundLibrary.NoRageAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.PassiveRageAudioClip uac in soundLibrary.PassiveRageAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.ActiveRageAudioClip uac in soundLibrary.ActiveRageAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.OniAudioClip uac in soundLibrary.OniAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.BigOniAudioClip uac in soundLibrary.BigOniAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.YureiAudioClip uac in soundLibrary.YureiAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.OrbAudioClip uac in soundLibrary.OrbAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }
        foreach (SoundLibrary.FinalBossAudioClip uac in soundLibrary.FinalBossAudioClips)
        {
            uac.SetType((int)uac.soundType);
        }

        //Music
        foreach (MusicLibrary.MusicSampleClip c in musicLibrary.MusicSampleClips)
        {
            c.SetType((int)c.musicSampleType);
        }
        foreach (MusicLibrary.MusicEffectClip c in musicLibrary.MusicEffectClips)
        {
            c.SetType((int)c.musicEffectType);
        }
    }*/
}
