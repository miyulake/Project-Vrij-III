using UnityEngine;

public class StateManager : MonoBehaviour
{
    public EntityState CurrentState { get; private set; } = EntityState.IDLE;
    public event System.Action<EntityState> OnStateChanged;

    public void SetState(EntityState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }
}
