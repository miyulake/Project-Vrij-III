using UnityEngine;
using UnityEngine.UI;

public class HealthVisuals : MonoBehaviour
{
    [SerializeField] private HealthUI m_PlayerOneUI;
    [SerializeField] private HealthUI m_PlayerTwoUI;

    [SerializeField] private AnimationCurve m_DrainCurve;
    [SerializeField] private float m_DrainDuration = 0.5f;

    [SerializeField] private float m_ShakeDuration = 0.2f;
    [SerializeField] private float m_ShakeStrength = 10f;

    private void Start()
    {
        m_PlayerOneUI.Initialize();
        m_PlayerTwoUI.Initialize();
    }

    private void Update()
    {
        m_PlayerOneUI.UpdateAll(Time.deltaTime, m_DrainDuration, m_ShakeDuration, m_ShakeStrength, m_DrainCurve);
        m_PlayerTwoUI.UpdateAll(Time.deltaTime, m_DrainDuration, m_ShakeDuration, m_ShakeStrength, m_DrainCurve);
    }

    [System.Serializable]
    private class HealthUI
    {
        public StateManager state;
        public Slider health;
        public Slider damageHealth;

        private float start;
        private float target;
        private float timer;

        private float lastHealth;
        private float shakeTimer;
        private Vector2 originalPos;
        private RectTransform barTransform;

        public void Initialize()
        {
            barTransform = (RectTransform)health.transform;
            originalPos = barTransform.anchoredPosition;
            lastHealth = health.value;
            damageHealth.value = health.value;
        }

        public void UpdateAll(float deltaTime, float drainDuration, float shakeDuration, float shakeStrength, AnimationCurve drainCurve)
        {
            UpdateDamage(deltaTime, drainDuration, drainCurve);
            UpdateShake(deltaTime, shakeDuration, shakeStrength);
            DetectHealthChange(shakeDuration);
        }

        private void UpdateDamage(float deltaTime, float drainDuration, AnimationCurve drainCurve)
        {
            if (damageHealth.value > health.value && !state.IsInStun())
            {
                if (health.value != target)
                {
                    start = damageHealth.value;
                    target = health.value;
                    timer = 0;
                }

                timer += deltaTime;
                var time = Mathf.Clamp01(timer / drainDuration);
                damageHealth.value = Mathf.Lerp(start, target, drainCurve.Evaluate(time));
            }
        }

        private void UpdateShake(float deltaTime, float shakeDuration, float shakeStrength)
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= deltaTime;

                var time = shakeTimer / shakeDuration;
                var offset = Random.insideUnitCircle * (time * shakeStrength);

                barTransform.anchoredPosition = originalPos + offset;

                if (shakeTimer <= 0) barTransform.anchoredPosition = originalPos;
            }
        }

        private void DetectHealthChange(float shakeDuration)
        {
            if (health.value != lastHealth)
            {
                shakeTimer = shakeDuration;
                lastHealth = health.value;
            }
        }
    }
}