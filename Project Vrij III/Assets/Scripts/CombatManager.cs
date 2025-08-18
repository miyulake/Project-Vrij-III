using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float comboInputTime = 0.33f;
    [SerializeField] private GameObject shield;
    private Controls controls;
    private Hitbox[] hitboxes;
    private static readonly int idleHash = Animator.StringToHash("Idle");
    private static readonly int blockHash = Animator.StringToHash("Shield");
    private int comboIndex = 0;
    private float comboTimer = 0f;

    private void Awake()
    {
        controls = new Controls();
        controls.Player.AttackForward.performed  += ctx => UseDirectionalAttack(AttackType.ATTACK_FORWARD);
        controls.Player.AttackDownward.performed += ctx => UseDirectionalAttack(AttackType.ATTACK_DOWNWARD);
        controls.Player.AttackUpward.performed   += ctx => UseDirectionalAttack(AttackType.ATTACK_UPWARD);
        controls.Player.ComboAttack.performed    += ctx => HandleComboAttack();
        controls.Player.Shield.performed         += ctx => HandleBlock(true);
        controls.Player.Shield.canceled          += ctx => HandleBlock(false);

        hitboxes = GetComponentsInChildren<Hitbox>(true);
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update() => HandleComboTimer();

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
        if (!IsInState(animator, idleHash)) return;
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
        if (!IsInState(animator, idleHash) && !IsInState(animator, blockHash)) return;
        animator.SetBool("IsBlocking", isShielding);
        shield.SetActive(isShielding);
    }

    private void ApplyAttackInfo(string attackName)
    {
        var hash = Animator.StringToHash(attackName);
        if (AttackDatabase.Data.TryGetValue(hash, out var info))
        {
            foreach (var hitbox in hitboxes) hitbox.SetAttackInfo(info);
        }
    }


    public static bool IsInState(Animator animator, int hash) => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hash;
}
