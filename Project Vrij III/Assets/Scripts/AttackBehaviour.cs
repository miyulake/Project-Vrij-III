using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    public AttackInfo attackInfo;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
        => animator.GetComponentInParent<CombatManager>().ApplyAttackInfo(attackInfo);
}
