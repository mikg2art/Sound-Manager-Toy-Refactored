using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

[CreateAssetMenu(fileName = "SoundLibrarySO", menuName = "SoundSystem/Sound/SoundLibrary")]
public class SoundLibrary : ScriptableObject
{
    [System.Serializable]
    public class ProtoAudioClip
    {
        //ProtoAudioClip is a base class for all audio clips
        [HideInInspector] public int type;
        [SerializeField]
        public AudioClip audioClip;
        public Setup setup;

        [HideInInspector] public bool showSetup = false;

        [System.Serializable]
        [SerializeField]
        public class Setup
        {
            [Range(0, 256)] public int priority = 124;
            [Range(0, 100)] public int volume = 100;
            [Range(-3, 3)] public float pitch = 1;
            public float startTime;
            public float endTime;

            [HideInInspector] public bool show3DSettings = false;

            public S3DSettings s3DSettings;
            [System.Serializable]
            public class S3DSettings
            {
                public float minDistance;
                public float maxDistance;
            }
        }

        public virtual void SetType(int _type) => type = _type;
    }

    //To place all audio clips I create an array of their type of clips
    [Header("Background Sound Effects")]
    [SerializeField]
    public BackgroundAudioClip[] BackgroundAudioClips;

    //And special class for, in this case, background audio clips. All childes of ProtoAudioClip
    [System.Serializable]
    public class BackgroundAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.Background soundType;
    }


    [Header("Environment Sound Effects")]
    public EnvironmentAudioClip[] EnvironmentAudioClips;

    [System.Serializable]
    public class EnvironmentAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.Environment soundType;
    }


    [Header("\nPlayer Sound Effects")]
    [Header("Player Base State")]
    public BaseAudioClip[] UniversalAudioClips;

    [System.Serializable]
    public class BaseAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.PlayerBaseState soundType;
    }


    [Header("Player No Rage State")]
    public NoRageAudioClip[] NoRageAudioClips;

    [System.Serializable]
    public class NoRageAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.PlayerNoRageState soundType;
    }


    [Header("Player Passive Rage State")]
    public PassiveRageAudioClip[] PassiveRageAudioClips;

    [System.Serializable]
    public class PassiveRageAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.PlayerPassiveRageState soundType;
    }


    [Header("Player Active Rage State")]
    public ActiveRageAudioClip[] ActiveRageAudioClips;

    [System.Serializable]
    public class ActiveRageAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.PlayerActiveRageState soundType;
    }


    [Header("\nEnemies SoundEffect: ")]
    [Header("Oni State")]
    public OniAudioClip[] OniAudioClips;

    [System.Serializable]
    public class OniAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.OniState soundType;
    }


    [Header("Big Oni State")]
    public BigOniAudioClip[] BigOniAudioClips;

    [System.Serializable]
    public class BigOniAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.BigOniState soundType;
    }


    [Header("Yurei State")]
    public YureiAudioClip[] YureiAudioClips;

    [System.Serializable]
    public class YureiAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.YureiState soundType;
    }


    [Header("Orb State")]
    public OrbAudioClip[] OrbAudioClips;

    [System.Serializable]
    public class OrbAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.OrbState soundType;
    }

    [Header("Final Boss State")]
    public FinalBossAudioClip[] FinalBossAudioClips;

    [System.Serializable]
    public class FinalBossAudioClip : ProtoAudioClip
    {
        [Header("Type")]
        public SoundEffect.FinalBossState soundType;
    }
}
