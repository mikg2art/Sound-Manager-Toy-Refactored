using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicLibrarySO", menuName = "SoundSystem/Music/MusicLibrary")]
public class MusicLibrary : ScriptableObject
{
    public class ProtoMusicClip
    {
        [HideInInspector] public int type;
        public AudioClip audioClip;
        public float transitionTime;
        public Setup setup;

        [System.Serializable]
        public class Setup
        {
            [Range(0, 256)] public int priority = 124;
            [Range(0, 100)] public int volume = 100;
            [Range(-3, 3)] public float pitch = 1;
            public float startTime;
            public float endTime;
        }
        public void SetType(int _type) { type = _type; }
    }

    [Header("MusicSamples")]
    public MusicSampleClip[] MusicSampleClips;

    [System.Serializable]
    public class MusicSampleClip : ProtoMusicClip
    {
        [Header("Type")]
        public MusicStates musicSampleType;
    }
}
