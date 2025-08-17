using System.Collections.Generic;

public static class AttackDatabase
{
    public static readonly Dictionary<int, AttackInfo> Data = new()
    {
        { AnimationHashes.Idle,           new AttackInfo { strength = 0f, hitstunDuration = 0f } },
        { AnimationHashes.comboOne,       new AttackInfo { strength = 5f, hitstunDuration = 0.3f } },
        { AnimationHashes.comboTwo,       new AttackInfo { strength = 5f, hitstunDuration = 0.3f } },
        { AnimationHashes.comboThree,     new AttackInfo { strength = 5f, hitstunDuration = 0.3f } },
        { AnimationHashes.attackForward,  new AttackInfo { strength = 5f, hitstunDuration = 0.3f } },
        { AnimationHashes.attackDownward, new AttackInfo { strength = 5f, hitstunDuration = 0.3f } },
        { AnimationHashes.attackUpward,   new AttackInfo { strength = 5f, hitstunDuration = 0.3f } }
    };
}
