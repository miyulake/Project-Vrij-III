using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_GenericCrossfade = 0.1f;
    [Range(1, 10)] [SerializeField] private int m_BufferFrames = 10;

    private InputReader m_InputReader;
    private StateManager m_StateManager;
    private Hitbox[] m_Hitboxes;
    private MoveData[] m_AllMoves;

    private MoveData m_CurrentMove;
    private int m_CurrentFrame;

    private MoveData m_BufferedMove;
    private float m_BufferedCrossfade;

    private void Start()
    {
        m_InputReader = GetComponent<InputReader>();
        m_StateManager = GetComponent<StateManager>();
        m_Hitboxes = GetComponentsInChildren<Hitbox>(true);
        m_AllMoves = Resources.LoadAll<MoveData>("MoveData");
    }

    private void Update()
    {
        // If we are hit reset everything and return
        if (m_StateManager.IsInStun())
        {
            EndMove();
            return;
        }
        // No current move -> check idle start or buffer
        if (m_CurrentMove == null)
        {
            // Check buffer first at the end of a move
            if (m_BufferedMove != null)
            {
                StartMove(m_BufferedMove, m_GenericCrossfade);
                return;
            }
            // Check input and perform move from idle
            var idleMove = CheckForInitialInput();
            if (idleMove != null)
            {
                StartMove(idleMove);
                return;
            }
            return;
        }

        ++m_CurrentFrame;

        CheckPostMoveBuffer();
        HandleCancelBuffering();
        HandleCancelExecution();

        // Track what state we are in based on the current frame
        if (m_CurrentFrame > m_CurrentMove.frames.TotalFrames()) 
        {
            EndMove();
            m_StateManager.SetState(EntityState.IDLE);
            return;
        }
        if (m_CurrentMove.frames.IsRecovering(m_CurrentFrame) && m_StateManager.CurrentState != EntityState.RECOVER)  
            m_StateManager.SetState(EntityState.RECOVER);
    }

    /// <summary>
    /// Plays animation according to MoveData
    /// </summary>
    public void StartMove(MoveData move, float crossfade = 0f)
    {
        if (move == null) return;

        m_StateManager.SetState(EntityState.ATTACK);
        m_CurrentMove = move;
        m_CurrentFrame = 0;

        if (!string.IsNullOrEmpty(move.animationName))
        {
            if (crossfade > 0f) m_Animator.CrossFade(move.animationName, crossfade, 0, 0f);
            else m_Animator.Play(move.animationName, 0, 0f);
        }

        SetMoveData(move);
        m_BufferedMove = null;
        m_BufferedCrossfade = 0f;
    }

    /// <summary>
    /// Resets move variables
    /// </summary>
    private void EndMove()
    {
        m_CurrentMove = null;
        m_BufferedCrossfade = 0f;
    }

    /// <summary>
    /// Idle input checks
    /// </summary>
    private MoveData CheckForInitialInput()
    {
        if (!AnimatorUtils.IsInAnyState(m_Animator, AnimationHashes.Idle)) return null;

        for (int i = 0; i < m_AllMoves.Length; i++)
        {
            var move = m_AllMoves[i];
            if (!move.startFromIdle) continue;
            if (WasInputPressed(move.input)) return move;
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
            AttackInput.JAB      => m_InputReader.ComboAttack,
            AttackInput.FORWARD  => m_InputReader.AttackForward,
            AttackInput.DOWNWARD => m_InputReader.AttackDownward,
            AttackInput.UPWARD   => m_InputReader.AttackUpward,
            AttackInput.GRAB     => m_InputReader.Grabbing,
            AttackInput.SNAP     => m_InputReader.Snap,
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
}
