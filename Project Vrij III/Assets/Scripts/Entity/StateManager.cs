using UnityEngine;

public class StateManager : MonoBehaviour
{
    public EntityState CurrentState { get; private set; } = EntityState.IDLE;
    public void SetState(EntityState newState) => CurrentState = newState;
    public bool IsInNeutral() =>
        CurrentState == EntityState.IDLE ||
        CurrentState == EntityState.BLOCK;

    public bool IsInStun() =>
        CurrentState == EntityState.HITSTUN ||
        CurrentState == EntityState.BLOCKSTUN;
}