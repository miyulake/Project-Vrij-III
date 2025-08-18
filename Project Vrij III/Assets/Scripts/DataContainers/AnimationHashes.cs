using UnityEngine;

public static class AnimationHashes
{
    public static readonly int Idle           = Animator.StringToHash("Idle");
    public static readonly int comboOne       = Animator.StringToHash("Combo_Attack_1");
    public static readonly int comboTwo       = Animator.StringToHash("Combo_Attack_2");
    public static readonly int comboThree     = Animator.StringToHash("Combo_Attack_3");
    public static readonly int attackForward  = Animator.StringToHash("Attack_Forward");
    public static readonly int attackDownward = Animator.StringToHash("Attack_Downward");
    public static readonly int attackUpward   = Animator.StringToHash("Attack_Upward");
}