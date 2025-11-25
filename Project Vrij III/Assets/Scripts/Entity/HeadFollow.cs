using UnityEngine;

public class HeadFollow : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_SmoothTime = 0.1f;
    [SerializeField] float m_DragMultiplier = 0.2f;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_TargetVelocity;
    private Vector3 m_Velocity;

    private void LateUpdate() => HeadMovement();

    private void HeadMovement()
    {
        // Unscaled prevents dividing by 0 when paused
        m_TargetVelocity = (m_Target.position - m_LastTargetPosition) / Time.unscaledDeltaTime; 

        var smoothTarget = m_Target.position - m_TargetVelocity * m_DragMultiplier;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            smoothTarget,
            ref m_Velocity,
            m_SmoothTime
        );

        m_LastTargetPosition = m_Target.position;
    }
}