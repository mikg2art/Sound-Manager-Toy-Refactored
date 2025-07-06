using UnityEngine;

[CreateAssetMenu(fileName = "CustomInstructions", menuName = "SoundSystem/Music/CustomInstructions")]
public class CustomInstructions : CustomizedSoundManagerBase
{
    public override MusicStates GetNextMusicState(MusicStates currentState)
    {
        switch (currentState)
        {
            case MusicStates.MusicLevel0Start:
                return MusicStates.MusicLevel0Continue;

            case MusicStates.MusicLevel2Start:
            case MusicStates.MusicLevel2Part2:
                return MusicStates.MusicLevel2Part1;

            case MusicStates.MusicLevel2Part1:
                return MusicStates.MusicLevel2Part2;

            case MusicStates.MusicLevel2End:
                return MusicStates.MusicLevel3Start;

            case MusicStates.MusicLevel3Start:
                return MusicStates.MusicLevel3Continue;

            case MusicStates.MusicLevel4Start:
                return MusicStates.MusicLevel4Part1;

            case MusicStates.MusicLevel4End:
                return MusicStates.MusicLevel5Final;

            case MusicStates.MusicLevel5Final:
                return MusicStates.MusicMenu;
        }

        return currentState;
    }
}
