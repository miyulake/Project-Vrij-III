using UnityEngine;

public class AttackManager : MonoBehaviour
{
    // HEAVY ATTACKS WITH "Right Mouse" + "Direction" AND LIGHT ATTACK STRING TOGETHER WITH "Left Mouse"
    public AttackManager Instance { get; private set; }
    [SerializeField] private Animator animator;
    [SerializeField] private float bufferDuration = 0.3f;
    private AttackType? bufferedAttack = null;
    private float bufferTimer = 0f;
    private readonly int idleHash = Animator.StringToHash("Idle");

    private Controls controls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        controls = new Controls();

        controls.Player.LightAttack.performed  += ctx => Attack(AttackType.H_PUNCH_1);
        controls.Player.HeavyAttack.performed  += ctx => Attack(AttackType.H_PUNCH_3);
    }
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update() => CheckBuffer();

    private void Attack(AttackType type)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == idleHash) HandleAttackAnimation(type);
        else
        {
            bufferedAttack = type;
            bufferTimer = bufferDuration;
        }
    }

    public void HandleAttackAnimation(AttackType type)
    {
        switch (type)
        {
            /*
            case AttackType.PUNCH_1:
                animator.Play("Hands_Punch", 0, 0);
                break;
            case AttackType.PUNCH_2:
                animator.Play("Hands_Punch2", 0, 0);
                break;
            case AttackType.PUNCH_3:
                animator.Play("Hands_Punch3", 0, 0);
                break;
            */
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
        print("Used: " + type);
    }

    private void CheckBuffer()
    {
        if (bufferedAttack.HasValue)
        {
            bufferTimer -= Time.deltaTime;

            if (bufferTimer <= 0f) bufferedAttack = null;
            else if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == idleHash)
            {
                HandleAttackAnimation(bufferedAttack.Value);
                bufferedAttack = null;
            }
        }
    }
}
