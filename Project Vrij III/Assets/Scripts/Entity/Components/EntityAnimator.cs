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
    public void PlayEnd(string animation) => m_Animator.Play(animation, 0, 1); // Le hack
    public void Pause() => m_Animator.speed = 0;
    public void Resume() => m_Animator.speed = 1;
}
