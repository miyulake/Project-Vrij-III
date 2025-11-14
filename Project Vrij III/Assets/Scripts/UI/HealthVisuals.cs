using UnityEngine;
using UnityEngine.UI;

public class HealthVisuals : MonoBehaviour
{
    [SerializeField] private StateManager m_P1, m_P2;
    [SerializeField] private Slider m_HealthP1, m_DamageHealthP1;
    [SerializeField] private Slider m_HealthP2, m_DamageHealthP2;
    [SerializeField] private AnimationCurve m_DrainCurve;
    [SerializeField] private float m_Duration = 0.5f;
    private float m_StartP1, m_TargetP1, m_TimerP1;
    private float m_StartP2, m_TargetP2, m_TimerP2;

    private void Start()
    {
        m_DamageHealthP1.value = m_HealthP1.value;
        m_DamageHealthP2.value = m_HealthP2.value;
    }

    private void Update() => UpdateDamageBars();

    private void UpdateDamageBars()
    {
        // Player 1
        if (m_DamageHealthP1.value > m_HealthP1.value && !m_P1.IsInStun())
        {
            if (m_HealthP1.value != m_TargetP1)
            {
                m_StartP1 = m_DamageHealthP1.value;
                m_TargetP1 = m_HealthP1.value;
                m_TimerP1 = 0;
            }
            m_TimerP1 += Time.deltaTime;
            m_DamageHealthP1.value = Mathf.Lerp(m_StartP1, m_TargetP1, m_DrainCurve.Evaluate(Mathf.Clamp01(m_TimerP1 / m_Duration)));
        }
        // Player 2
        if (m_DamageHealthP2.value > m_HealthP2.value && !m_P2.IsInStun())
        {
            if (m_HealthP2.value != m_TargetP2)
            {
                m_StartP2 = m_DamageHealthP2.value;
                m_TargetP2 = m_HealthP2.value;
                m_TimerP2 = 0;
            }
            m_TimerP2 += Time.deltaTime;
            m_DamageHealthP2.value = Mathf.Lerp(m_StartP2, m_TargetP2, m_DrainCurve.Evaluate(Mathf.Clamp01(m_TimerP2 / m_Duration)));
        }
    }
}