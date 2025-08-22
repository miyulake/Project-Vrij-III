using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    [SerializeField] private string attackName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var combat = animator.GetComponentInParent<CombatManager>();
        combat.ApplyAttackInfo(attackName);
    }
}
