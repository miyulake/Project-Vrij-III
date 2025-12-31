using Game.Entities;
using UnityEngine;

public class ShakeController : EntityComponent, ITickable, IResettable
{
    [SerializeField] private Transform m_ShakeTarget;
    private float m_ShakeDuration;
    private float m_ShakeMagnitude;
    private float m_ShakeTimer;
    private Vector3 m_OriginalPosition;

    public void Tick()
    {
        if (m_ShakeTimer > 0f)
        {
            m_ShakeTimer -= Time.deltaTime;

            var durationSafe = Mathf.Max(m_ShakeDuration, 0.0001f);
            var progress = Mathf.Clamp01(1f - (m_ShakeTimer / durationSafe));
            var currentMagnitude = m_ShakeMagnitude * Mathf.Sqrt(1f - progress);
            var offsetX = Random.Range(-1f, 1f) * currentMagnitude;
            var offsetY = Random.Range(-1f, 1f) * currentMagnitude;
            var shakeOffset = new Vector3(offsetX, offsetY, 0f);

            m_ShakeTarget.localPosition = m_OriginalPosition + shakeOffset;
            if (m_ShakeTimer <= 0f) m_ShakeTarget.localPosition = m_OriginalPosition;
        }
    }

    public void Reset()
    {
        m_ShakeTimer = 0f;
        m_ShakeDuration = 0f;
        m_ShakeMagnitude = 0f;

        m_OriginalPosition = Vector3.zero;
        m_ShakeTarget.localPosition = Vector3.zero;
    }

    public void TriggerShake(float duration, float magnitude)
    {
        if (m_ShakeTimer <= 0f) m_OriginalPosition = m_ShakeTarget.localPosition;

        m_ShakeDuration = duration;
        m_ShakeTimer = duration;
        m_ShakeMagnitude = magnitude;
    }
}
