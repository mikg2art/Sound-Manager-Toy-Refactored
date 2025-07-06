using UnityEngine;

public class CustomizedSoundManagerBase : ScriptableObject, ICustomizedSoundManager
{
    public virtual MusicStates GetNextMusicState(MusicStates currentState) => currentState;
}
