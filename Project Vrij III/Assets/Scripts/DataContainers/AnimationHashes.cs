using UnityEngine;

public static class AnimationHashes
{
    public static readonly int Idle           = Animator.StringToHash("Idle");
    public static readonly int comboOne       = Animator.StringToHash("Hands_Combo_Attack_1");
    public static readonly int comboTwo       = Animator.StringToHash("Hands_Combo_Attack_2");
    public static readonly int comboThree     = Animator.StringToHash("Hands_Combo_Attack_3");
    public static readonly int attackForward  = Animator.StringToHash("Hands_Attack_Forward");
    public static readonly int attackDownward = Animator.StringToHash("Hands_Attack_Downward");
    public static readonly int attackUpward   = Animator.StringToHash("Hands_Attack_Upward");
}