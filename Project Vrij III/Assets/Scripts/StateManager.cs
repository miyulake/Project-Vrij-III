using UnityEngine;

public class StateManager : MonoBehaviour
{
    public EntityState CurrentState { get; private set; } = EntityState.IDLE;
    public event System.Action<EntityState> OnStateChanged;

    public void SetState(EntityState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        //OnStateChanged?.Invoke(newState);
    }

    public bool IsState(EntityState state) => CurrentState == state;
    public bool IsIdle => CurrentState == EntityState.IDLE;
    public bool IsBlocking => CurrentState == EntityState.BLOCKING;
    public bool IsAttacking => CurrentState == EntityState.ATTACKING;
    public bool IsHitstun => CurrentState == EntityState.HITSTUN;
    public bool IsDead => CurrentState == EntityState.DEAD;
}
