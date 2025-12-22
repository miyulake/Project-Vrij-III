using Game.Entities;
using UnityEngine;

public class ThrowHandler : EntityComponent, ITickable
{
    [SerializeField] MoveData m_ClankMove;
    [SerializeField] private GameObject m_ThrowAnchor;
    [SerializeField] private AnimationCurve m_TurnCurve;
    [Range(0.05f, 0.25f)] [SerializeField] private float m_TurnDuration = 0.2f;
    [SerializeField] private AudioClip m_CaughtSound;
    [SerializeField] private AudioClip m_ClankSound;
    private CapsuleCollider2D m_PlayerCollider;
    private float m_TurnTime = -1f;
    private float m_StartY;
    private bool m_GrabConnected;

    public override void Initialize(Entity entity)
    {
        base.Initialize(Entity);
        m_PlayerCollider = GetComponent<CapsuleCollider2D>();
    }

    public void Tick() => HandleThrow();

    private void HandleThrow()
    {
        var shouldThrow = 
            m_ThrowAnchor.activeSelf && 
            m_GrabConnected && 
            ThrowEligible() && 
            RoundManager.Instance.CurrentState != RoundState.INTRO;

        if (shouldThrow)
        {
            if (m_TurnTime < 0f) m_TurnTime = 0f;

            m_PlayerCollider.enabled = false;
            Entity.Opponent.transform.position = m_ThrowAnchor.transform.position;
        }
        else
        {
            m_PlayerCollider.enabled = true;
            m_TurnTime = -1f;
            m_GrabConnected = false;
        }
        if (!AttackComp.IsPaused) UpdateRotation(); // Absolute hack
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

    public bool ThrowEligible()
    {
        var opponentSM = Entity.Opponent.Get<StateMachine>();
        return opponentSM.CurrentState is HitStunState || opponentSM.CurrentState is CaughtState;
    }

    public void ConnectGrab() => m_GrabConnected = true;

    public MoveData GetClank() => m_ClankMove;
    public AudioClip GetCaughtSound() => m_CaughtSound;
    public AudioClip GetClankSound() => m_ClankSound;
}