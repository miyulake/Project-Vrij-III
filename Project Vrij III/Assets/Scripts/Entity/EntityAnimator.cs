using UnityEngine;

public class EntityAnimator : EntityComponent
{
    [SerializeField] private Animator m_Animator;

    public void Tick()
    {
        m_Animator.SetBool("InStun", Entity.StateMachine.IsInStun());
        m_Animator.SetBool("IsBlocking", Entity.StateMachine.CurrentState is BlockState);
    }

    public void Play(string animation) => m_Animator.Play(animation, 0, 0);
}
