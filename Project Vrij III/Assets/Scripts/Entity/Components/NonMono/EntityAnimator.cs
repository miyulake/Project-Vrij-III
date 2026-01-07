using UnityEngine;
using Game.Entities;

public class EntityAnimator : EntityContext, IEntityComponent, ITickable, IPausable, IResettable
{
    public bool IsPaused { get; private set; }
    private Animator m_Animator;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_Animator = ViewComp.Animator;
    }

    public void Tick()
    {
        m_Animator.SetBool("InStun", StateMachine.IsInStun());
        m_Animator.SetBool("IsBlocking", StateMachine.CurrentState is BlockState);
    }

    public void Play(string animation) => m_Animator.Play(animation, 0, 0);
    public void PlayCrossFade(string animation, float fade) => m_Animator.CrossFade(animation, fade, 0, 0);
    public void PlayEnd(string animation) => m_Animator.Play(animation, 0, 1); // HACK

    public void Pause()
    {
        m_Animator.speed = 0;
        IsPaused = true;
    }

    public void Resume()
    {
        m_Animator.speed = 1;
        IsPaused = false;
    }

    public void Reset() => Play("Start");
}
