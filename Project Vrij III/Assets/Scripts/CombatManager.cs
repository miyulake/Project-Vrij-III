using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float comboInputTime = 0.33f;
    [SerializeField] private GameObject shield;

    private InputReader input;
    private StateManager stateManager;
    private Hitbox[] hitboxes;

    private static readonly int idleHash = Animator.StringToHash("Idle");
    private static readonly int blockHash = Animator.StringToHash("Shield");

    private int comboIndex = 0;
    private float comboTimer = 0f;

    private void Awake()
    {
        input = GetComponent<InputReader>();
        stateManager = GetComponent<StateManager>();
        hitboxes = GetComponentsInChildren<Hitbox>(true);
    }

    private void Update()
    {
        if (stateManager.CurrentState == EntityState.DEAD) return;
        HandleComboTimer();
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (input.comboAttack) HandleComboAttack();
        if (input.attackForward) UseDirectionalAttack(AttackType.ATTACK_FORWARD);
        if (input.attackDownward) UseDirectionalAttack(AttackType.ATTACK_DOWNWARD);
        if (input.attackUpward) UseDirectionalAttack(AttackType.ATTACK_UPWARD);
        HandleBlock(input.blocking);
    }

    private void HandleComboAttack()
    {
        if (comboIndex == 3) return;
        ++comboIndex;
        comboTimer = 0f;
        animator.SetInteger("ComboIndex", comboIndex);

        switch (comboIndex)
        {
            case 1: ApplyAttackInfo("Combo_Attack_1"); break;
            case 2: ApplyAttackInfo("Combo_Attack_2"); break;
            case 3: ApplyAttackInfo("Combo_Attack_3"); break;
        }
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
        if (!stateManager.IsIdle) return;
        stateManager.SetState(EntityState.ATTACKING);

        switch (type)
        {
            case AttackType.ATTACK_FORWARD:
                ApplyAttackInfo("Attack_Forward");
                animator.Play("Attack_Forward", 0, 0);
                break;
            case AttackType.ATTACK_DOWNWARD:
                ApplyAttackInfo("Attack_Downward");
                animator.Play("Attack_Downward", 0, 0);
                break;
            case AttackType.ATTACK_UPWARD:
                ApplyAttackInfo("Attack_Upward");
                animator.Play("Attack_Upward", 0, 0);
                break;
        }
    }

    private void HandleBlock(bool isShielding)
    {
        if (!stateManager.IsState(EntityState.ATTACKING) || !stateManager.IsState(EntityState.HITSTUN))
        animator.SetBool("IsBlocking", isShielding);
        shield.SetActive(isShielding);
    }

    private void ApplyAttackInfo(string attackName)
    {
        var hash = Animator.StringToHash(attackName);
        if (AttackDatabase.Data.TryGetValue(hash, out var info))
        {
            foreach (var hitbox in hitboxes)
                hitbox.SetAttackInfo(info);
        }
    }

    public static bool IsInState(Animator animator, int hash) =>
        animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hash;
}
