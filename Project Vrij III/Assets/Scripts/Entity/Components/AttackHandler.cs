using Game.Entities;
using UnityEngine;

public class AttackHandler : EntityComponent, ITickable, IResettable
{
    public bool IsPaused { get; private set; } = false;

    [Header("Buffer Settings")]
    [SerializeField] private float m_BufferCrossfade = 0.1f;
    [Range(1, 10)] [SerializeField] private int m_BufferFrames = 10;

    private Hitbox[] m_Hitboxes;
    private MoveData[] m_AllMoves;

    private MoveData m_CurrentMove;
    private int m_CurrentFrame;

    private MoveData m_BufferedMove;
    private float m_BufferedCrossfade;

    public override void Initialize(Entity entity)
    {
        base.Initialize(Entity);
        m_Hitboxes = GetComponentsInChildren<Hitbox>(true);
        m_AllMoves = Resources.LoadAll<MoveData>("MoveData");
    }

    /// <summary>
    /// Used in fixed update to ensure time step syncs with 60fps
    /// </summary>
    public void Tick()
    {
        // Only go through logic if the game is going or unpaused
        if (StateMachine.CurrentState is DeadState ||
            RoundManager.Instance.CurrentState == RoundState.INTRO ||
            GameManager.Instance.IsPaused()) return;

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

    /// <summary>
    /// Plays animation according to MoveData
    /// </summary>
    private void StartMove(MoveData move, float crossfade = 0f)
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

    /// <summary>
    /// Resets move variables. If a buffered move exists, start it immediately
    /// </summary>
    private void EndMove()
    {
        for (int i = 0; i < m_Hitboxes.Length; i++)
            m_Hitboxes[i].gameObject.SetActive(false);

        // Check buffer at the end of the current move
        if (m_BufferedMove != null)
        {
            StartMove(m_BufferedMove, m_BufferCrossfade);
            return;
        }

        // No buffered move = clear current move
        m_CurrentMove = null;
        m_BufferedCrossfade = 0f;
    }

    /// <summary>
    /// Activate hitboxes during the active frames of the current move
    /// </summary>
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

    /// <summary>
    /// Idle input checks
    /// </summary>
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

    /// <summary>
    /// Stores any move pressed during current move in the buffer to execute after current move ends
    /// </summary>
    private void CheckPostMoveBuffer()
    {
        // Only allow buffering during the buffer window of the current move
        if (m_BufferedMove != null || m_CurrentFrame < m_CurrentMove.frames.TotalFrames() - m_BufferFrames) return;

        for (int i = 0; i < m_AllMoves.Length; i++)
        {
            if (WasInputPressed(m_AllMoves[i].input)) m_BufferedMove = m_AllMoves[i];
        }
    }

    /// <summary>
    /// Stores next move when cancel input is pressed
    /// </summary>
    private void HandleCancelBuffering()
    {
        if (m_CurrentMove.cancelOptions == null || m_CurrentMove.cancelOptions.Length == 0) return;

        if (TryCheckForCancelInput(out MoveData nextMove, out float crossfade))
        {
            m_BufferedMove = nextMove;
            m_BufferedCrossfade = crossfade;
        }
    }

    /// <summary>
    /// Execute buffered moves during cancel window
    /// </summary>
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

    /// <summary>
    /// Cancel input check
    /// </summary>
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

    /// <summary>
    /// Input check helper
    /// </summary>
    private bool WasInputPressed(AttackInput inputType)
    {
        return inputType switch
        {
            AttackInput.JAB      => Input.ComboAttack,
            AttackInput.FORWARD  => Input.AttackForward,
            AttackInput.DOWNWARD => Input.AttackDownward,
            AttackInput.UPWARD   => Input.AttackUpward,
            AttackInput.GRAB     => Input.Grab,
            AttackInput.SNAP     => Input.Snap,
            AttackInput.PUSH     => Input.Push,
            AttackInput.TAUNT    => Input.Taunt,
            AttackInput.SUPER    => Input.Super,
            _                    => false
        };
    }

    /// <summary>
    /// Apply MoveData to hitboxes
    /// </summary>
    private void SetMoveData(MoveData move)
    {
        for (int i = 0; i < m_Hitboxes.Length; i++) m_Hitboxes[i].MoveData = move;
    }

    public void Reset()
    {
        m_CurrentMove = null;
        m_CurrentFrame = 0;

        m_BufferedMove = null;
        m_BufferedCrossfade = 0;
    }

    public void SetPauseState(bool isPaused) => IsPaused = isPaused;
}