using Game.Entities;
using UnityEngine;

public class StateMachine : EntityContext, IEntityComponent, ITickable, IResettable
{
    public EntityState CurrentState { get; private set; }
    private StateFactory m_StateFactory;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
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

    public void Reset() => ChangeState<IdleState>(true);
}