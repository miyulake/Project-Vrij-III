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
        m_TargetVelocity = (m_Target.position - m_LastTargetPosition) / Time.deltaTime;
        m_LastTargetPosition = m_Target.position;

        var lagTarget = m_Target.position - m_TargetVelocity * m_DragMultiplier;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            lagTarget,
            ref m_Velocity,
            m_SmoothTime
        );
    }
}