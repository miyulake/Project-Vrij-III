using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public AttackManager Instance { get; private set; }
    [SerializeField] private Animator animator;
    [SerializeField] private float comboInputTime = 0.33f;
    private Controls controls;
    private static readonly int idleHash = Animator.StringToHash("Idle");
    private static readonly int lastComboHash = Animator.StringToHash("Hands_Combo_Attack_3");
    private int comboIndex = 0;
    private float comboTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        controls = new Controls();

        controls.Player.AttackForward.performed += ctx => UseDirectionalAttack(AttackType.ATTACK_FORWARD);
        controls.Player.AttackDownward.performed += ctx => UseDirectionalAttack(AttackType.ATTACK_DOWNWARD);
        controls.Player.AttackUpward.performed += ctx => UseDirectionalAttack(AttackType.ATTACK_UPWARD);
        controls.Player.ComboAttack.performed += ctx => HandleComboAttack();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        HandleComboTimer();
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
        if (!IsInState(animator, idleHash)) return;
        switch (type)
        {
            case AttackType.ATTACK_FORWARD:
                animator.Play("Hands_Attack_Forward", 0, 0);
                break;
            case AttackType.ATTACK_DOWNWARD:
                animator.Play("Hands_Attack_Downward", 0, 0);
                break;
            case AttackType.ATTACK_UPWARD:
                animator.Play("Hands_Attack_Upward", 0, 0);
                break;
        }
        Debug.Log("Used directional attack: " + type);
    }

    public static bool IsInState(Animator animator, int hash) => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hash;
}
