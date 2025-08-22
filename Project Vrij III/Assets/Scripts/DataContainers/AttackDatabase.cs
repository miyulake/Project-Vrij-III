using UnityEngine;
using System.Collections.Generic;

public static class AttackDatabase
{
    public static readonly Dictionary<int, AttackInfo> Data = new()
    {
        { AnimationHashes.Idle,           new AttackInfo() },
        { AnimationHashes.comboOne,       new AttackInfo { knockback = new(1,0), hitstunDuration = 0.3f, shakeMagnitude = 0.02f } },
        { AnimationHashes.comboTwo,       new AttackInfo { knockback = new(2,0), hitstunDuration = 0.3f, shakeMagnitude = 0.04f } },
        { AnimationHashes.comboThree,     new AttackInfo { knockback = new(3,0), hitstunDuration = 0.3f, shakeMagnitude = 0.07f } },
        { AnimationHashes.attackForward,  new AttackInfo { knockback = new(5,0), hitstunDuration = 0.5f, shakeMagnitude = 0.1f } },
        { AnimationHashes.attackDownward, new AttackInfo { knockback = new(5,-10), hitstunDuration = 0.5f, shakeMagnitude = 0.125f } },
        { AnimationHashes.attackUpward,   new AttackInfo { knockback = new(5,5), hitstunDuration = 1f, shakeMagnitude = 0.125f } }
    };
}
