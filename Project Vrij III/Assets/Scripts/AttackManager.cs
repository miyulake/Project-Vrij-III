using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public AttackManager Instance { get; private set; }
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
        controls.Player.MediumAttack.performed += ctx => Attack(AttackType.H_PUNCH_2);
        controls.Player.HeavyAttack.performed  += ctx => Attack(AttackType.H_PUNCH_3);
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    public void Attack(AttackType type)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

        switch (type)
        {
            case AttackType.PUNCH_1:
                animator.Play("Hands_Punch", 0, 0);
                break;
            case AttackType.PUNCH_2:
                animator.Play("Hands_Punch2", 0, 0);
                break;
            case AttackType.PUNCH_3:
                animator.Play("Hands_Punch3", 0, 0);
                break;
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
}
