using UnityEngine;

public class StateManager : MonoBehaviour
{
    public EntityState CurrentState { get; private set; } = EntityState.IDLE;

    public void SetState(EntityState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
    }

    public bool IsState(EntityState state) => CurrentState == state;

    public bool IsIdle => CurrentState == EntityState.IDLE;
    public bool IsBlocking => CurrentState == EntityState.BLOCKING;
    public bool IsAttacking => CurrentState == EntityState.ATTACKING;
    public bool IsHitstun => CurrentState == EntityState.HITSTUN;
    public bool IsGrabbed => CurrentState == EntityState.GRABBED;
}
