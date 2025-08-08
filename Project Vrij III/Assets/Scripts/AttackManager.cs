using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public AttackManager Instance { get; private set; }
    [SerializeField] private Animator animator;
    [SerializeField] private float comboInputTime = 0.33f;
    private Controls controls;
    private static readonly int idleHash = Animator.StringToHash("Idle");
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

        controls.Player.LightAttack.performed += ctx => HandleComboAttack();
        controls.Player.HeavyAttack.performed += ctx => PlaySpecialAttack(AttackType.H_PUNCH_3);
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update() => HandleComboTimer();

    private void HandleComboAttack()
    {
        ++comboIndex;

        if (comboIndex > 3) comboIndex = 1;
        comboTimer = 0f;

        PlayComboAttack(comboIndex);
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
            }
        }
    }

    private void PlayComboAttack(int index)
    {
        switch (index)
        {
            case 1:
                animator.Play("Hands_Punch", 0, 0);
                break;
            case 2:
                animator.Play("Hands_Punch2", 0, 0);
                break;
            case 3:
                animator.Play("Hands_Punch3", 0, 0);
                break;
        }
        Debug.Log("Combo index: " + index);
    }

    private void PlaySpecialAttack(AttackType type)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != idleHash) return;

        switch (type)
        {
            case AttackType.H_PUNCH_1:
                animator.Play("Hands_PunchH1", 0, 0);
                break;
            case AttackType.H_PUNCH_2:
                animator.Play("Hands_PunchH2", 0, 0);
                break;
            case AttackType.H_PUNCH_3:
                animator.Play("Hands_PunchH3", 0, 0);
                break;
        }
        Debug.Log("Used special attack: " + type);
    }
}
