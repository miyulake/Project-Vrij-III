using System.Collections.Generic;

public static class AttackDatabase
{
    public static readonly Dictionary<int, AttackInfo> Data = new()
    {
        { AnimationHashes.Idle,           new AttackInfo() },
        { AnimationHashes.comboOne,       new AttackInfo { strength = 2f, hitstunDuration = 0.3f, shakeMagnitude = 0.03f } },
        { AnimationHashes.comboTwo,       new AttackInfo { strength = 3f, hitstunDuration = 0.3f, shakeMagnitude = 0.05f } },
        { AnimationHashes.comboThree,     new AttackInfo { strength = 5f, hitstunDuration = 0.3f, shakeMagnitude = 0.075f } },
        { AnimationHashes.attackForward,  new AttackInfo { strength = 15f, hitstunDuration = 0.5f, shakeMagnitude = 0.1f } },
        { AnimationHashes.attackDownward, new AttackInfo { strength = 20f, hitstunDuration = 1f, shakeMagnitude = 0.125f } },
        { AnimationHashes.attackUpward,   new AttackInfo { strength = 25f, hitstunDuration = 1f, shakeMagnitude = 0.125f } }
    };
}
