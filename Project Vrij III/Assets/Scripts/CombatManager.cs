using System;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float comboInputTime = 0.33f;
    [SerializeField] private float attackHoldDuration = 0.2f;
    [SerializeField] private GameObject shield;

    private InputReader input;
    private Hitbox[] hitboxes;

    private int comboIndex = 0;
    private float comboTimer = 0f;
    private float attackHoldTime = 0f;

    private void Awake()
    {
        input = GetComponent<InputReader>();
        hitboxes = GetComponentsInChildren<Hitbox>(true);
    }

    private void Update()
    {
        HandleComboTimer();
        HandleInputs();

        if (input.AttackUpward) attackHoldTime += Time.deltaTime;
        else attackHoldTime = 0f;
    }

    private void HandleInputs()
    {
        if (input.ComboAttack) HandleComboAttack();
        if (input.AttackForward) UseDirectionalAttack(AttackType.ATTACK_FORWARD);
        if (input.AttackDownward) UseDirectionalAttack(AttackType.ATTACK_DOWNWARD);
        if (attackHoldTime > attackHoldDuration) UseDirectionalAttack(AttackType.ATTACK_UPWARD);
        HandleBlock(input.Blocking);
    }

    private void HandleComboAttack()
    {
        if (comboIndex == 3) return;
        ++comboIndex;
        comboTimer = 0f;
        animator.SetInteger("ComboIndex", comboIndex);
    }

    private void HandleComboTimer()
    {
        if (comboIndex > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboInputTime)
            {
                comboTimer = 0f;
                comboIndex = 0;
                animator.SetInteger("ComboIndex", comboIndex);
            }
        }
    }

    private void UseDirectionalAttack(AttackType type)
    {
        if (!AnimatorUtils.IsInAnyState(animator, AnimationHashes.Idle)) return;

        switch (type)
        {
            case AttackType.ATTACK_FORWARD:
                animator.Play("Attack_Forward", 0, 0);
                break;
            case AttackType.ATTACK_DOWNWARD:
                animator.Play("Attack_Downward", 0, 0);
                break;
            case AttackType.ATTACK_UPWARD:
                animator.Play("Attack_Upward", 0, 0);
                break;
        }
    }

    private void HandleBlock(bool isShielding)
    {
        if (!AnimatorUtils.IsInAnyState(animator, AnimationHashes.Idle) &&
            !AnimatorUtils.IsInAnyState(animator, AnimationHashes.Block)) return;
        animator.SetBool("IsBlocking", isShielding);
        shield.SetActive(isShielding);
    }

    public void ApplyAttackInfo(AttackInfo attackInfo)
    {
        if (attackInfo == null) return;
        foreach (var hitbox in hitboxes) hitbox.SetAttackInfo(attackInfo);
    }
}