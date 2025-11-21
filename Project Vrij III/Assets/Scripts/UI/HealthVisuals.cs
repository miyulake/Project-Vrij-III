using UnityEngine;
using UnityEngine.UI;

public class HealthVisuals : MonoBehaviour
{
    [SerializeField] private HealthUI m_PlayerOneUI;
    [SerializeField] private HealthUI m_PlayerTwoUI;
    [SerializeField] private HealthUIConfig m_Config;

    private void Start()
    {
        m_PlayerOneUI.Initialize();
        m_PlayerTwoUI.Initialize();
    }

    private void Update()
    {
        m_PlayerOneUI.UpdateAll(Time.deltaTime, m_Config, PlayerManager.Instance.playerOne);
        m_PlayerTwoUI.UpdateAll(Time.deltaTime, m_Config, PlayerManager.Instance.playerTwo);
    }

    [System.Serializable]
    private class HealthUIConfig
    {
        public float drainDuration = 0.5f;
        public float shakeDuration = 0.2f;
        public float shakeStrength = 10f;
        public AnimationCurve drainCurve;
    }

    [System.Serializable]
    private class HealthUI
    {
        public Slider health;
        public Slider ghostHealth;

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
            ghostHealth.value = health.value;
        }

        public void UpdateAll(float deltaTime, HealthUIConfig config, EntityManager manager)
        {
            health.value = manager.Entity.CurrentHealth;
            UpdateGhostHealth(deltaTime, config, manager);
            UpdateShake(deltaTime, config.shakeDuration, config.shakeStrength);
            DetectHealthChange(config.shakeDuration);
        }

        private void UpdateGhostHealth(float deltaTime, HealthUIConfig config, EntityManager manager)
        {
            if (manager.Entity.RecievedComboHits == 1)
            {
                ghostHealth.value = health.value;
                return;
            }

            if (ghostHealth.value > health.value && !manager.State.IsInStun())
            {
                if (health.value != target)
                {
                    start = ghostHealth.value;
                    target = health.value;
                    timer = 0;
                }

                timer += deltaTime;
                var time = Mathf.Clamp01(timer / config.drainDuration);
                ghostHealth.value = Mathf.Lerp(start, target, config.drainCurve.Evaluate(time));
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