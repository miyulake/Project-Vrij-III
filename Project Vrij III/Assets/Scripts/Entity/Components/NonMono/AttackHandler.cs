using Game.Entities;
using UnityEngine;

public class AttackHandler : EntityContext, IEntityComponent, ITickable, IResettable, IPausable
{
    public bool IsPaused { get; private set; }

    private CombatSettings m_Settings;

    private Hitbox[] m_Hitboxes;
    private MoveData[] m_AllMoves;

    private MoveData m_CurrentMove;
    private int m_CurrentFrame;

    private MoveData m_BufferedMove;
    private float m_BufferedCrossfade;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_Settings = Entity.Character.combat;
        m_Hitboxes = Entity.GetComponentsInChildren<Hitbox>(true);
        m_AllMoves = Entity.Character.AllMoves;
    }

    public void Tick()
    {
        // Only go through logic if the game is ongoing
        if (StateMachine.CurrentState is DeadState || RoundManager.Instance.CurrentState == RoundState.INTRO) return;
        if (Time.timeScale < 1f) return; // Hack

        // If we are hit or the round ended reset everything and return
        if (StateMachine.IsInStun())
        {
            Reset();
            return;
        }

        // No current move = check idle start or buffer
        if (m_CurrentMove == null)
        {
            var idleMove = CheckForInitialInput();
            if (idleMove != null) StartMove(idleMove);
            return;
        }
        
        // Check logic
        //HandleHitboxActivation();
        CheckPostMoveBuffer();
        HandleCancelBuffering();
        HandleCancelExecution();

        // Track what state we are in based on the current frame
        if (m_CurrentFrame >= m_CurrentMove.frames.TotalFrames()) // NOTE: >= Hack I think?
        {
            EndMove();
            // If a buffered move didn't start and we are not idle, return to idle
            if (m_CurrentMove == null && StateMachine.CurrentState is not IdleState) 
                StateMachine.ChangeState<IdleState>();
        }
        else if (m_CurrentMove.frames.IsRecovering(m_CurrentFrame) && 
            StateMachine.CurrentState is not RecoverState)
            StateMachine.ChangeState<RecoverState>();

        // Advance frame
        if (!IsPaused) ++m_CurrentFrame;
    }

    public void StartMove(MoveData move, float crossfade = 0f)
    {
        if (move == null) return;

        StateMachine.ChangeState<AttackState>();
        m_CurrentMove = move;
        m_CurrentFrame = 0;

        if (!string.IsNullOrEmpty(move.animationName))
        {
            if (crossfade > 0f) AnimatorComp.PlayCrossFade(move.animationName, crossfade);
            else AnimatorComp.Play(move.animationName);
        }

        SetMoveData(move);
        m_BufferedMove = null;
        m_BufferedCrossfade = 0f;
    }

    private void EndMove()
    {
        for (int i = 0; i < m_Hitboxes.Length; i++)
            m_Hitboxes[i].gameObject.SetActive(false);

        // Check buffer at the end of the current move
        if (m_BufferedMove != null)
        {
            StartMove(m_BufferedMove, m_Settings.bufferCrossfade);
            return;
        }

        // No buffered move = clear current move
        m_CurrentMove = null;
        m_BufferedCrossfade = 0f;
    }

    private void HandleHitboxActivation()
    {
        if (m_CurrentMove == null) return;

        var isActive = m_CurrentMove.frames.IsActive(m_CurrentFrame);
        var activeIndices = m_CurrentMove.hitboxIndices;

        for (int i = 0; i < m_Hitboxes.Length; i++)
        {
            var shouldBeActive = isActive && activeIndices != null && System.Array.IndexOf(activeIndices, i) >= 0;
            m_Hitboxes[i].gameObject.SetActive(shouldBeActive);
        }
    }

    private MoveData CheckForInitialInput()
    {
        if (StateMachine.CurrentState is not IdleState) return null;

        for (int i = 0; i < m_AllMoves.Length; i++)
        {
            if (!m_AllMoves[i].startFromIdle) continue;
            if (WasInputPressed(m_AllMoves[i].input)) return m_AllMoves[i];
        }
        return null;
    }

    private void CheckPostMoveBuffer()
    {
        // Only allow buffering during the buffer window of the current move
        if (m_BufferedMove != null || m_CurrentFrame < m_CurrentMove.frames.TotalFrames() - m_Settings.bufferFrames) return;

        for (int i = 0; i < m_AllMoves.Length; i++)
            if (WasInputPressed(m_AllMoves[i].input)) m_BufferedMove = m_AllMoves[i];
    }

    private void HandleCancelBuffering()
    {
        if (m_CurrentMove.cancelOptions == null || m_CurrentMove.cancelOptions.Length == 0) return;

        if (TryCheckForCancelInput(out MoveData nextMove, out float crossfade))
        {
            m_BufferedMove = nextMove;
            m_BufferedCrossfade = crossfade;
        }
    }

    private void HandleCancelExecution()
    {
        if (m_BufferedMove == null || m_CurrentMove.cancelOptions == null) return;

        // Check if the buffered move is allowed in this cancel window
        var canCancel = false;
        for (int i = 0; i < m_CurrentMove.cancelOptions.Length; i++)
        {
            if (m_CurrentMove.cancelOptions[i].move == m_BufferedMove)
            {
                canCancel = true;
                break;
            }
        }
        // Execute the move only if its in cancel options and within the cancel window
        if (canCancel &&
            m_CurrentFrame >= m_CurrentMove.frames.cancel.start &&
            m_CurrentFrame <= m_CurrentMove.frames.cancel.end)
        {
            StartMove(m_BufferedMove, m_BufferedCrossfade);
        }
    }

    private bool TryCheckForCancelInput(out MoveData move, out float crossfade)
    {
        var options = m_CurrentMove.cancelOptions;
        for (int i = 0; i < options.Length; i++)
        {
            var option = options[i];
            if (option.move != null && WasInputPressed(option.move.input))
            {
                move = option.move;
                crossfade = option.crossfadeDuration;
                return true;
            }
        }

        move = null;
        crossfade = 0f;
        return false;
    }

    private bool WasInputPressed(AttackInput inputType)
    {
        return inputType switch
        {
            AttackInput.JAB      => InputComp.ComboAttack,
            AttackInput.FORWARD  => InputComp.AttackForward,
            AttackInput.DOWNWARD => InputComp.AttackDownward,
            AttackInput.UPWARD   => InputComp.AttackUpward,
            AttackInput.GRAB     => InputComp.Grab,
            AttackInput.SNAP     => InputComp.Snap,
            AttackInput.PUSH     => InputComp.Push,
            AttackInput.TAUNT    => InputComp.Taunt,
            _                    => false
        };
    }

    private void SetMoveData(MoveData move)
    {
        for (int i = 0; i < m_Hitboxes.Length; i++) m_Hitboxes[i].SetMoveData(move);
    }

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;

    public void Reset()
    {
        m_CurrentMove = null;
        m_CurrentFrame = 0;

        m_BufferedMove = null;
        m_BufferedCrossfade = 0;
    }
}