using UnityEngine;

public class EntityOrientation : EntityComponent
{
    [SerializeField] private AnimationCurve m_TurnCurve;
    [SerializeField] private float m_TurnDuration = 0.2f;

    private Vector3 m_OriginalPosition;
    private Quaternion m_OriginalRotation;

    private Vector3 m_StoredPosition; 
    private Quaternion m_StoredRotation;

    private int m_CurrentFacing = 1;
    private float m_TurnTime = -1f;
    private float m_StartY;

    public int FacingDirection => transform.position.x < Entity.Opponent.transform.position.x ? 1 : -1;

    protected override void Awake()
    {
        base.Awake();
        m_OriginalPosition = transform.position;
        m_OriginalRotation = transform.rotation;
    }

    public void Tick()
    {
        CheckTurnNeeded();
        UpdateTurnRotation();
    }

    public void CheckTurnNeeded()
    {
        if (!Entity.StateMachine.IsNeutral()) return;

        if (FacingDirection != m_CurrentFacing)
        {
            m_CurrentFacing = FacingDirection;
            m_TurnTime = 0f;
            m_StartY = transform.localEulerAngles.y;
        }
    }

    private void UpdateTurnRotation()
    {
        if (m_TurnTime < 0f) return;

        m_TurnTime += Time.deltaTime;

        var time = Mathf.Clamp01(m_TurnTime / m_TurnDuration);
        var targetY = m_CurrentFacing == 1 ? 0f : 180f;
        var newY = Mathf.Lerp(m_StartY, targetY, m_TurnCurve.Evaluate(time));

        transform.localRotation = Quaternion.Euler(0, newY, 0);

        if (!Entity.StateMachine.IsNeutral())
        {
            m_CurrentFacing = FacingDirection;
            transform.localRotation = Quaternion.Euler(0f, m_CurrentFacing == 1 ? 0f : 180f, 0f);
            m_TurnTime = 1f;
        }

        if (time >= 1f) m_TurnTime = -1f;
    }

    public void StoreOrientation()
    {
        m_StoredPosition = new Vector3(transform.position.x, transform.position.y, 0);
        m_StoredRotation = Quaternion.Euler(0f, m_CurrentFacing == 1 ? 0f : 180f, 0f);
    }

    public void ApplyStoredOrientation() => transform.SetPositionAndRotation(m_StoredPosition, m_StoredRotation);

    public void Reset() => transform.SetPositionAndRotation(m_OriginalPosition, m_OriginalRotation);
}
