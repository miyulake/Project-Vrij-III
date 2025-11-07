using UnityEngine;

public class StateManager : MonoBehaviour
{
    public EntityState CurrentState { get; private set; } = EntityState.IDLE;

    public void SetState(EntityState newState) => CurrentState = newState;
    public bool IsInState(EntityState state) => CurrentState == state;
}
