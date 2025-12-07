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

    public void ChangeState<T>(bool forceOverride = false, params object[] args) where T : EntityState
    {
        if (CurrentState is DeadState && !forceOverride) return;

        CurrentState?.OnExit();
        CurrentState = m_StateFactory.Create<T>(args);
        CurrentState.OnEnter();

        Debug.Log($"{Entity.gameObject.name} changed to: {CurrentState.GetType().Name}");
    }

    public void Tick() => CurrentState?.Tick();

    public bool IsInStun() => 
        CurrentState is HitStunState || CurrentState is BlockStunState || CurrentState is CaughtState;

    public bool IsNeutral() => 
        CurrentState is IdleState || CurrentState is BlockState;
}