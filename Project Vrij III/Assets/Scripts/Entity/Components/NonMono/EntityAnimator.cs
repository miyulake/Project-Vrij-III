using Game.Entities;

public class EntityAnimator : EntityContext, IEntityComponent, ITickable, IPausable, IResettable
{
    public bool IsPaused { get; private set; }

    public void Initialize(Entity entity) => SetEntity(entity);

    public void Tick()
    {
        ViewComp.Animator.SetBool("InStun", StateMachine.IsInStun());
        ViewComp.Animator.SetBool("IsBlocking", StateMachine.CurrentState is BlockState);
    }

    public void Play(string animation) => ViewComp.Animator.Play(animation, 0, 0);
    public void PlayCrossFade(string animation, float fade) => ViewComp.Animator.CrossFade(animation, fade, 0, 0);
    public void PlayEnd(string animation) => ViewComp.Animator.Play(animation, 0, 1); // HACK

    public void Pause()
    {
        ViewComp.Animator.speed = 0;
        IsPaused = true;
    }

    public void Resume()
    {
        ViewComp.Animator.speed = 1;
        IsPaused = false;
    }

    public void Reset() => Play("Start");
}
