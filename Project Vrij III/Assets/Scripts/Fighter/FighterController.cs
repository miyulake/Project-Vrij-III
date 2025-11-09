using UnityEngine;

public class FighterController : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;

    private InputReader m_InputReader;
    private Hitbox[] m_Hitboxes;
    private MoveData[] m_AllMoves;

    private MoveData m_CurrentMove;
    private int m_CurrentFrame;

    private MoveData m_BufferedMove;
    private float m_BufferedCrossfade;

    private void Start()
    {
        m_InputReader = GetComponent<InputReader>();
        m_Hitboxes = GetComponentsInChildren<Hitbox>(true);
        m_AllMoves = Resources.LoadAll<MoveData>("MoveData");
    }

    private void Update()
    {
        // ─────────────────────────────
        // NO CURRENT MOVE → CHECK IDLE START
        // ─────────────────────────────
        if (m_CurrentMove == null)
        {
            var move = CheckForInitialInput();
            if (move != null) StartMove(move);
            return;
        }

        // ─────────────────────────────
        // ACTIVE MOVE
        // ─────────────────────────────
        ++m_CurrentFrame;

        HandleCancelBuffering();
        HandleBufferedExecution();

        if (m_CurrentFrame > m_CurrentMove.frames.TotalFrames()) EndMove();
    }

    // ============================================================
    //  MOVE START
    // ============================================================
    public void StartMove(MoveData move, float crossfade = 0f)
    {
        if (move == null) return;

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

    // ============================================================
    //  MOVE END
    // ============================================================
    private void EndMove()
    {
        m_CurrentMove = null;
        m_BufferedMove = null;
        m_BufferedCrossfade = 0f;
    }

    // ============================================================
    //  IDLE INPUT CHECKS
    // ============================================================
    private MoveData CheckForInitialInput()
    {
        // Only from Idle state
        if (!AnimatorUtils.IsInAnyState(m_Animator, AnimationHashes.Idle)) return null;

        for (int i = 0; i < m_AllMoves.Length; i++)
        {
            var move = m_AllMoves[i];
            if (!move.startFromIdle) continue;
            if (WasInputPressed(move.input)) return move;
        }
        return null;
    }

    // ============================================================
    //  CANCEL BUFFERING
    // ============================================================
    private void HandleCancelBuffering()
    {
        if (m_CurrentMove.cancelOptions == null || m_CurrentMove.cancelOptions.Length == 0) return;

        if (TryCheckForCancelInput(out MoveData nextMove, out float crossfade))
        {
            m_BufferedMove = nextMove;
            m_BufferedCrossfade = crossfade;
        }
    }

    // ============================================================
    //  EXECUTE BUFFERED MOVES DURING CANCEL WINDOW
    // ============================================================
    private void HandleBufferedExecution()
    {
        if (m_BufferedMove == null) return;

        if (m_CurrentFrame >= m_CurrentMove.frames.cancel.start &&
            m_CurrentFrame <= m_CurrentMove.frames.cancel.end)
            StartMove(m_BufferedMove, m_BufferedCrossfade);
    }

    // ============================================================
    //  CANCEL INPUT CHECK
    // ============================================================
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

    // ============================================================
    //  INPUT CHECK HELPER
    // ============================================================
    private bool WasInputPressed(AttackInput inputType)
    {
        return inputType switch
        {
            AttackInput.JAB => m_InputReader.ComboAttack,
            AttackInput.FORWARD => m_InputReader.AttackForward,
            AttackInput.DOWNWARD => m_InputReader.AttackDownward,
            AttackInput.UPWARD => m_InputReader.AttackUpward,
            AttackInput.GRAB => m_InputReader.Grabbing,
            AttackInput.SNAP => m_InputReader.Snap,
            _ => false
        };
    }

    // ============================================================
    //  APPLY MOVEDATA TO HITBOXES
    // ============================================================
    private void SetMoveData(MoveData move)
    {
        foreach (var hitbox in m_Hitboxes) hitbox.MoveData = move;
    }
}
