
    /*public enum MusicSamples
    {
        ML0Start, ML0p1, ML0p2,
        ML1p1, ML1p2,
        ML2Start, ML2p1, ML2p2, ML2Death1, ML2Death2, ML2End,
        ML3Start, ML3p1, ML3p2, ML3Death1, ML3Death2,
        ML4Start, ML4p1_1, ML4p1_2, ML4p2_1, ML4p2_2, ML4End,
        ML5Fin,
        ML1End_p1, ML1End_p2, Menu,
        ML3bS1, ML3bS2, ML3b1, ML3b2, ML3bD1, ML3bD2, ML3e1, ML3e2,
        Menu1, Menu2, ML1Death1, ML1Death2
    }*/

    public enum MusicStates
    {
        None = -1,

        MusicMenu = 0,

        MusicLevel0Start = 1,
        MusicLevel0Continue = 2,
        MusicLevel0End = 9,

        MusicLevel1Continue = 11,
        MusicLevel1End = 19,

        MusicLevel2Start = 21,
        MusicLevel2Part1 = 22,
        MusicLevel2Part2 = 23,
        MusicLevel2End = 29,

        MusicLevel3Start = 31,
        MusicLevel3Continue = 32,

        MusicLevel4Start = 41,
        MusicLevel4Part1 = 42,
        MusicLevel4Part2 = 43,
        MusicLevel4End = 49,

        MusicLevel5Final = 51
    }
