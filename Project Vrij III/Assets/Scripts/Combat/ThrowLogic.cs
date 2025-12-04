using UnityEngine;

public class ThrowLogic : EntityComponent
{
    public MoveData clankMove;
    [SerializeField] private GameObject m_ThrowAnchor;
    [SerializeField] private AnimationCurve m_TurnCurve;
    [Range(0.05f, 0.25f)] [SerializeField] private float m_TurnDuration = 0.2f;
    private CapsuleCollider2D m_PlayerCollider;
    private float m_TurnTime = -1f;
    private float m_StartY;

    protected override void Awake()
    {
        base.Awake();
        m_PlayerCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update() => HandleThrow();

    private void HandleThrow()
    {
        if (m_ThrowAnchor.activeSelf && 
            (Entity.Opponent.StateMachine.CurrentState is HitStunState ||
            Entity.Opponent.StateMachine.CurrentState is CaughtState) &&
            RoundManager.Instance.CurrentState != RoundState.INTRO)
        {
            if (m_TurnTime < 0f) m_TurnTime = 0f;

            m_PlayerCollider.enabled = false;
            Entity.Opponent.transform.position = m_ThrowAnchor.transform.position;
        }
        else
        {
            m_PlayerCollider.enabled = true;
            m_TurnTime = -1f;
        }
        UpdateRotation();
    }


    private void UpdateRotation()
    {
        if (m_TurnTime < 0f)
        {
            m_StartY = transform.localEulerAngles.y;
            return;
        }

        m_TurnTime += Time.deltaTime;

        var time = Mathf.Clamp01(m_TurnTime / m_TurnDuration);
        var newY = Mathf.Lerp(0f, 180f, m_TurnCurve.Evaluate(time));
        transform.localRotation = Quaternion.Euler(0f, m_StartY + newY, 0f);
    }
}