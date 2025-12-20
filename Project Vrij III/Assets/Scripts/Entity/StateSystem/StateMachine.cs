using Game.Entities;
using UnityEngine;

public class StateMachine : EntityComponent, ITickable
{
    public EntityState CurrentState { get; private set; }
    private StateFactory m_StateFactory;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        m_StateFactory = new StateFactory(entity);
    }

    public void Tick() => CurrentState?.Tick();

    public void ChangeState<T>(bool forceOverride = false, params object[] args) where T : EntityState
    {
        if (CurrentState is DeadState && !forceOverride) return;

        CurrentState?.OnExit();
        CurrentState = m_StateFactory.Create<T>(args);
        CurrentState.OnEnter();

        Debug.Log($"{Entity.gameObject.name} changed to: {CurrentState.GetType().Name}");
    }

    public bool IsInStun() => 
        CurrentState is HitStunState || CurrentState is BlockStunState || CurrentState is CaughtState;

    public bool IsNeutral() => 
        CurrentState is IdleState || CurrentState is BlockState;
}