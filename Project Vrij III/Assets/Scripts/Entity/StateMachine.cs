using UnityEngine;

public class StateMachine : EntityComponent
{
    public EntityState CurrentState { get; private set; }
    private StateFactory m_StateFactory;

    protected override void Awake()
    {
        base.Awake();
        m_StateFactory = new StateFactory(Entity, this);
    }

    public void ChangeState<T>(params object[] args) where T : EntityState
    {
        if (CurrentState is DeadState) return;
        CurrentState?.OnExit();
        CurrentState = m_StateFactory.Create<T>(args);
        Debug.Log($"{Entity.gameObject.name} changed to: {CurrentState.GetType().Name}");
        CurrentState.OnEnter();
    }

    public void OverrideChangeState<T>(params object[] args) where T : EntityState
    {
        CurrentState?.OnExit();
        CurrentState = m_StateFactory.Create<T>(args);
        Debug.Log($"{Entity.gameObject.name} changed to: {CurrentState.GetType().Name}");
        CurrentState.OnEnter();
    }

    public void Tick() => CurrentState?.Tick();

    public bool IsInStun() => CurrentState is HitStunState || CurrentState is BlockStunState;
    public bool IsNeutral() => CurrentState is IdleState || CurrentState is BlockState;
}