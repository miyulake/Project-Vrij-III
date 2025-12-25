using Game.Entities.Resources;
using UnityEngine;
using UnityEngine.UI;

public class HealthVisualsUI : MonoBehaviour
{
    [SerializeField] private HealthUI m_PlayerOneUI;
    [SerializeField] private HealthUI m_PlayerTwoUI;
    [SerializeField] private HealthUIConfig m_Config;

    private void Start()
    {
        BindPlayer(PlayerManager.Instance.playerOne, m_PlayerOneUI);
        BindPlayer(PlayerManager.Instance.playerTwo, m_PlayerTwoUI);
    }

    private void Update()
    {
        if (RoundManager.Instance.CurrentState == RoundState.INTRO)
        {
            // Hack
            m_PlayerOneUI.Reset();
            m_PlayerTwoUI.Reset();
            return;
        }

        var playerOne = PlayerManager.Instance.playerOne;
        var playerTwo = PlayerManager.Instance.playerTwo;
        m_PlayerOneUI.Tick(Time.deltaTime, playerOne.Get<StateMachine>().IsInStun());
        m_PlayerTwoUI.Tick(Time.deltaTime, playerTwo.Get<StateMachine>().IsInStun());
    }

    private void BindPlayer(Entity player, HealthUI ui)
    {
        ui.Unbind();
        var resources = player.Get<EntityResources>();
        ui.Bind(resources.Health, m_Config);
    }

    private void OnDestroy()
    {
        m_PlayerOneUI.Unbind();
        m_PlayerTwoUI.Unbind();
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

        private float m_Start;
        private float m_Target;
        private float m_Timer;

        private bool m_IsStunned;

        private float m_ShakeTimer;
        private Vector2 m_OriginalPosition;
        private RectTransform m_BarTransform;

        private HealthUIConfig m_Config;
        private IResource m_Health;

        private void Initialize(IResource resource, HealthUIConfig config)
        {
            m_Health = resource;
            m_Config = config;

            m_BarTransform = (RectTransform)health.transform;
            m_OriginalPosition = m_BarTransform.anchoredPosition;

            health.maxValue = m_Health.Max;
            ghostHealth.maxValue = m_Health.Max;

            health.value = m_Health.Current;
            ghostHealth.value = m_Health.Current;
        }

        public void Bind(IResource resource, HealthUIConfig config)
        {
            Initialize(resource, config);
            m_Health.Changed += OnHealthChanged;
            m_Health.Emptied += OnHealthEmptied;
        }

        public void Unbind()
        {
            if (m_Health != null)
            {
                m_Health.Changed -= OnHealthChanged;
                m_Health.Emptied -= OnHealthEmptied;
                m_Health = null;
            }
        }

        private void OnHealthChanged(int current, int max)
        {
            var lastHealthValue = health.value;

            health.value = current;
            if (current < ghostHealth.value)
            {
                if (!m_IsStunned) ghostHealth.value = lastHealthValue;

                m_Start = ghostHealth.value;
                m_Target = current;
                m_Timer = 0;

                m_ShakeTimer = m_Config.shakeDuration;
            }
        }

        private void OnHealthEmptied() 
        {
            // TO-DO: End animation, etc
        }

        public void Tick(float deltaTime, bool isStunned)
        {
            m_IsStunned = isStunned;
            UpdateGhostHealth(deltaTime, isStunned);
            UpdateShake(deltaTime);
        }

        private void UpdateGhostHealth(float deltaTime, bool isStunned)
        {
            if (ghostHealth.value <= health.value || isStunned) return;

            m_Timer += deltaTime;
            var time = Mathf.Clamp01(m_Timer / m_Config.drainDuration);
            ghostHealth.value = Mathf.Lerp(m_Start, m_Target, m_Config.drainCurve.Evaluate(time));
        }

        private void UpdateShake(float deltaTime)
        {
            if (m_ShakeTimer <= 0) return;

            m_ShakeTimer -= deltaTime;
            var time = m_ShakeTimer / m_Config.shakeDuration;
            var offset = Random.insideUnitCircle * (time * m_Config.shakeStrength);
            m_BarTransform.anchoredPosition = m_OriginalPosition + offset;

            if (m_ShakeTimer <= 0) m_BarTransform.anchoredPosition = m_OriginalPosition;
        }

        public void Reset()
        {
            health.value = m_Health.Max;
            ghostHealth.value = m_Health.Max;

            m_Timer = 0;
            m_Start = ghostHealth.value;
            m_Target = ghostHealth.value;

            m_ShakeTimer = 0;
            if (m_BarTransform != null) m_BarTransform.anchoredPosition = m_OriginalPosition;
        }
    }
}
