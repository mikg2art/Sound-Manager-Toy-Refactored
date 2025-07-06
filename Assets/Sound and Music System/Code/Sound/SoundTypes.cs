public interface SoundEffect
{
    public enum Background
    {
        None = 0,
        HeartBeat = 1, 
        FirstLocationBackground = 2, 
        SecondLocationBackground = 3,
    }
    public enum Environment { 
        None = 0,
        Waterfall = 1, 
        River = 2, 
        OrbNoise = 3, 
        Roots = 4, 
    }

    public enum PlayerBaseState
    {
        None = 0,
        Idle = 1,
        ReceivingDamage = 2,
        ReceivingAcidDamage = 3,
        ReceivingFireDamage = 4,
        Parry = 5,
        Death = 6
    }
    public enum PlayerNoRageState
    {
        None = 0,
        FirstAttack = 1,
        SecondAttack = 2,
        ThirdAttack = 3,
        Dash = 4,
        Slash = 5,
        RageDeactivate = 6
    }
    public enum PlayerPassiveRageState
    {
        None = 0,
        FirstAttack = 1,
        SecondAttack = 2,
        ThirdAttack = 3,
        Dash = 4,
        Slash = 5
    }
    public enum PlayerActiveRageState
    {
        None = 0,
        FirstAttack = 1,
        SecondAttack = 2,
        ThirdAttack = 3,
        Dash = 4,
        Slash = 5,
        RageActivate = 6
    }
    public enum OniState
    {
        None = 0,
        Attack = 1,
        ReceivingDamage = 2,
        TalkOne = 3,
        TalkTwo = 4,
        TalkThree = 5,
        Death = 6,
        Dash = 7
    }
    public enum BigOniState
    {
        None = 0,
        Attack = 1,
        ReceivingDamage = 2,
        TalkOne = 3,
        TalkTwo = 4,
        TalkThree = 5,
        Death = 6,
        Dash = 7
    }
    public enum YureiState
    {
        None = 0,
        Attack = 1,
        ReceivingDamage = 2,
        TalkOne = 3,
        TalkTwo = 4,
        TalkThree = 5,
        Death = 6
    }
    public enum OrbState
    {
        None = 0,
        Idle = 1,
        ReceivingDamage = 2,
        Destroyed = 3,
        RootsDying = 4
    }
    public enum FinalBossState
    {
        None = 0,
        AppearanceIntro = 1,
        Scream = 2,
        AttackSweepOne = 3,
        AttackSweepTwo = 4,
        CenterSmash = 5,
        SlamLeftHandArea = 6,
        SlamRightHandArea = 7,
        SmashOne = 8,
        SmashTwo = 9,
        SummonOne = 10,
        SummonTwo = 11,
        ArmReceivingDamage = 12,
        HeadReceivingDamage = 13,
        FallDown = 14,
        Death = 15
    }
}