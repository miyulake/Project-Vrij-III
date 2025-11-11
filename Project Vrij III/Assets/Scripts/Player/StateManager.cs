using UnityEngine;

public class StateManager : MonoBehaviour
{
    public FighterState CurrentState { get; private set; } = FighterState.IDLE;
    public void SetState(FighterState newState) => CurrentState = newState;
    public void ExitStun()
    {
        switch (CurrentState)
        {
            case FighterState.HITSTUN:
                SetState(FighterState.IDLE);
                break;
            case FighterState.BLOCKSTUN:
                SetState(FighterState.BLOCK);
                break;
        }
    }
}
