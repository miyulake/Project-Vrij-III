using UnityEngine;

public static class AnimatorUtils
{
    public static bool IsInAnyState(Animator animator, params int[] hashes)
    {
        var current = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        foreach (var hash in hashes) if (current == hash) return true;
        return false;
    }
}