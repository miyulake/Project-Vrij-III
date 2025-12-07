using UnityEngine;

public class EntityOrientation : EntityComponent
{
    [SerializeField] private AnimationCurve m_TurnCurve;
    [SerializeField] private float m_TurnDuration = 0.2f;

    private Vector3 m_OriginalPosition;
    private Quaternion m_OriginalRotation;

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

        if (!Entity.StateMachine.IsNeutral()) ForceFixOrientation();
    }

    public void ForceFixOrientation()
    {
        m_CurrentFacing = FacingDirection;

        var forcedY = m_CurrentFacing == 1 ? 0f : 180f;
        var newRotation = transform.localRotation = Quaternion.Euler(0f, forcedY, 0f); // Fix rotation
        var position = transform.localPosition;
        var newPosition = transform.localPosition = new Vector3(position.x, position.y, 0); // Fix position

        transform.SetLocalPositionAndRotation(newPosition, newRotation);

        m_TurnTime = -1f;
    }

    public void Reset() => transform.SetPositionAndRotation(m_OriginalPosition, m_OriginalRotation);
}
