using Game.Entities;
using UnityEngine;

public class EntityOrientation : EntityContext, IEntityComponent, ITickable, IResettable
{
    private OrientationSettings m_Settings;

    private Vector3 m_OriginalPosition;
    private Quaternion m_OriginalRotation;

    private int m_CurrentFacing = 1;
    private float m_TurnTime = -1f;
    private float m_StartY;

    private bool m_ManualSmoothTurn = false;

    public int FacingDirection => Entity.transform.position.x < Opponent.transform.position.x ? 1 : -1;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_Settings = Entity.Character.orientation;
        m_OriginalPosition = Entity.transform.position;
        m_OriginalRotation = Entity.transform.rotation;
    }

    public void Tick()
    {
        CheckTurnNeeded();
        UpdateTurnRotation();
    }

    private void CheckTurnNeeded()
    {
        if (!StateMachine.IsNeutral()) return;

        if (FacingDirection != m_CurrentFacing)
        {
            m_CurrentFacing = FacingDirection;
            m_TurnTime = 0f;
            m_StartY = Entity.transform.eulerAngles.y;
        }
    }

    private void UpdateTurnRotation()
    {
        if (m_TurnTime < 0f) return;

        m_TurnTime += Time.deltaTime;

        var time = Mathf.Clamp01(m_TurnTime / m_Settings.turnDuration);
        var targetY = m_CurrentFacing == 1 ? 0f : 180f;
        var newY = Mathf.Lerp(m_StartY, targetY, m_Settings.turnCurve.Evaluate(time));

        Entity.transform.rotation = Quaternion.Euler(0, newY, 0);

        if (!StateMachine.IsNeutral() && !m_ManualSmoothTurn)
        {
            ForceFixOrientation();
            return;
        }

        if (m_TurnTime >= m_Settings.turnDuration)
        {
            m_TurnTime = -1f;
            m_ManualSmoothTurn = false;
        }
    }

    public void ForceFixOrientation()
    {
        m_CurrentFacing = FacingDirection;

        var forcedY = m_CurrentFacing == 1 ? 0f : 180f;
        var newRotation = Quaternion.Euler(0f, forcedY, 0f); // Fix rotation
        var newPosition = new Vector3(Entity.transform.position.x, Entity.transform.position.y, 0); // Fix position

        Entity.transform.SetPositionAndRotation(newPosition, newRotation);

        m_TurnTime = -1f;
    }

    public void ManualTurn(bool smooth = false)
    {
        m_CurrentFacing *= -1;

        if (smooth)
        {
            m_TurnTime = 0f;
            m_StartY = Entity.transform.eulerAngles.y;
            m_ManualSmoothTurn = true;
        }
        else
        {
            var forcedY = m_CurrentFacing == 1 ? 0f : 180f;
            Entity.transform.rotation = Quaternion.Euler(0f, forcedY, 0f);
            m_TurnTime = -1f;
            m_ManualSmoothTurn = false;
        }
    }

    public void Reset() => Entity.transform.SetPositionAndRotation(m_OriginalPosition, m_OriginalRotation);
}
