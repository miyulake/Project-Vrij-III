using UnityEngine;

public static class AnimationHashes
{
    public static readonly int Idle            = Animator.StringToHash("Idle");
    public static readonly int Block           = Animator.StringToHash("Block_Loop");
    public static readonly int BlockStun       = Animator.StringToHash("Block_Stun");
    public static readonly int Stun            = Animator.StringToHash("Stun");
    public static readonly int Grab            = Animator.StringToHash("Grab");
    public static readonly int Snap            = Animator.StringToHash("Snap");
    public static readonly int Push            = Animator.StringToHash("Push");
    public static readonly int Taunt           = Animator.StringToHash("Taunt");
    public static readonly int comboOne        = Animator.StringToHash("Combo_Attack_1");
    public static readonly int comboTwo        = Animator.StringToHash("Combo_Attack_2");
    public static readonly int comboThree      = Animator.StringToHash("Combo_Attack_3");
    public static readonly int attackForward   = Animator.StringToHash("Attack_Forward");
    public static readonly int attackDownward  = Animator.StringToHash("Attack_Downward");
    public static readonly int attackUpward    = Animator.StringToHash("Attack_Upward");
}