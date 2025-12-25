using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CombatUI m_PlayerOneUI;
    [SerializeField] private CombatUI m_PlayerTwoUI;
    [SerializeField] private float m_DisplayDuration = 1f;

    private void Start()
    {
        m_PlayerOneUI.Unbind();
        m_PlayerTwoUI.Unbind();

        var playerOne = PlayerManager.Instance.playerOne;
        var playerTwo = PlayerManager.Instance.playerTwo;
        m_PlayerOneUI.Bind(playerTwo.Get<ComboTracker>(), playerTwo.Get<EntityResolver>(), m_DisplayDuration);
        m_PlayerTwoUI.Bind(playerOne.Get<ComboTracker>(), playerOne.Get<EntityResolver>(), m_DisplayDuration);
    }

    private void Update()
    {
        m_PlayerOneUI.Tick();
        m_PlayerTwoUI.Tick();
    }

    private void OnDestroy()
    {
        m_PlayerOneUI.Unbind();
        m_PlayerTwoUI.Unbind();
    }

    [System.Serializable]
    private class CombatUI
    {
        public TextMeshProUGUI attackText, comboText;
        private ComboTracker m_BoundCombo;
        private EntityResolver m_BoundResolver;

        private float m_DisplayTimer;
        private float m_DisplayDuration;

        public void Bind(ComboTracker combo, EntityResolver resolver, float displayDuration)
        {
            m_BoundCombo = combo;
            m_BoundResolver = resolver;
            m_DisplayDuration = displayDuration;

            m_BoundCombo.OnComboUpdated += OnComboUpdated;
            m_BoundResolver.OnHitTypeChanged += OnHitTypeChanged;
        }

        public void Unbind()
        {
            if (m_BoundCombo != null)
            {
                m_BoundCombo.OnComboUpdated -= OnComboUpdated;
                m_BoundCombo = null;
            }
            if (m_BoundResolver != null)
            {
                m_BoundResolver.OnHitTypeChanged -= OnHitTypeChanged;
                m_BoundResolver = null;
            }
        }

        private void OnComboUpdated(int hits, int damage)
        {
            if (hits <= 0) return;
            if (hits == 1)
            {
                attackText.text = "";
                comboText.text = "";
                return;
            }

            if (GameManager.Instance.CurrentMode != GameMode.PAINT)
                comboText.text = $"{hits} Hit Combo\n{damage} Damage";
            else
                comboText.text = $"{hits} Hit Combo";

            m_DisplayTimer = m_DisplayDuration;
        }

        private void OnHitTypeChanged(ContactType type)
        {
            attackText.text = type switch
            {
                ContactType.COUNTER => "Counter",
                ContactType.PUNISH => "Punish",
                _ => ""
            };
            m_DisplayTimer = m_DisplayDuration;
        }

        public void Tick()
        {
            if (m_DisplayTimer > 0)
            {
                m_DisplayTimer -= Time.deltaTime;
                if (m_DisplayTimer <= 0)
                {
                    attackText.text = "";
                    comboText.text = "";
                }
            }
        }
    }
}
