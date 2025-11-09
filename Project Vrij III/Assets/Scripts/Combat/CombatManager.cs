using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbodyTwoD;
    [SerializeField] private float comboInputTime = 0.33f;
    [SerializeField] private float attackHoldDuration = 0.2f;
    private InputReader input;
    private Hitbox[] hitboxes;
    private int comboIndex = 0;
    private float comboTimer = 0f;
    private float attackHoldTime = 0f;

    private void Start()
    {
        input = GetComponent<InputReader>();
        hitboxes = GetComponentsInChildren<Hitbox>(true);
    }

    private void Update()
    {
        if (input.Restart) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Debug

        HandleComboTimer();
        HandleInputs();
    }

    private void HandleInputs()
    {
        if (GameManager.Instance.MatchEnded) return; // HACK

        if (input.ComboAttack) HandleComboAttack();
        else if (input.AttackForward) UseDirectionalAttack(AttackType.ATTACK_FORWARD);
        else if (input.AttackDownward) UseDirectionalAttack(AttackType.ATTACK_DOWNWARD);
        else if (input.AttackUpward)
        {
            attackHoldTime += Time.deltaTime;
            if (attackHoldTime > attackHoldDuration) UseDirectionalAttack(AttackType.ATTACK_UPWARD);
        }
        else attackHoldTime = 0f;

        if (input.Grabbing) UseGrab();
        else if (input.Snap) UseSnap();

        HandleBlock(input.Blocking);
    }

    private void HandleComboAttack()
    {
        if (comboIndex == 3 || !AnimatorUtils.IsInAnyState(animator, 
            AnimationHashes.Idle, AnimationHashes.comboOne, AnimationHashes.comboTwo)) return;

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

    private void UseGrab()
    {
        if (!AnimatorUtils.IsInAnyState(animator, AnimationHashes.Idle)) return;
        animator.Play("Grab", 0, 0);
    }

    private void UseSnap()
    {
        if (!AnimatorUtils.IsInAnyState(animator, AnimationHashes.Idle)) return;
        animator.Play("Snap", 0, 0);
    }

    private void HandleBlock(bool isBlocking)
    {
        if (!AnimatorUtils.IsInAnyState(animator, AnimationHashes.Idle) &&
            !AnimatorUtils.IsInAnyState(animator, AnimationHashes.Block)) return;
        animator.SetBool("IsBlocking", isBlocking);
    }

    public void ApplyAttackInfo(AttackInfo attackInfo)
    {
        if (attackInfo == null) return;
        //foreach (var hitbox in hitboxes) hitbox.SetAttackInfo(attackInfo);
    }
}