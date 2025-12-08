using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CombatUI m_PlayerOneUI;
    [SerializeField] private CombatUI m_PlayerTwoUI;
    [SerializeField] private float m_DisplayDuration = 1f;

    private void Update()
    {
        m_PlayerOneUI.UpdateUI(PlayerManager.Instance.playerTwo, m_DisplayDuration);
        m_PlayerTwoUI.UpdateUI(PlayerManager.Instance.playerOne, m_DisplayDuration);
    }

    [System.Serializable]
    private class CombatUI
    {
        public TextMeshProUGUI attackText, comboText;
        private float m_DisplayTimer;

        public void UpdateUI(Entity entity, float timer)
        {
            if (entity.Combo.Hits == 1) // Reset on first hit and set after
            {
                attackText.text = "";
                comboText.text = "";
            }

            if (entity.StateMachine.CurrentState is HitStunState)
            {
                m_DisplayTimer = 0;

                if (entity.Resolver.HitType == ContactType.COUNTER) 
                    attackText.text = "Counter";
                else if (entity.Resolver.HitType == ContactType.PUNISH) 
                    attackText.text = "Punish";

                if (entity.Combo.Hits > 1)
                {
                    if (GameManager.Instance.CurrentMode != GameMode.PAINT)
                        comboText.text = $"{entity.Combo.Hits} Hit Combo\n{entity.Combo.Damage} Damage";
                    else
                        comboText.text = $"{entity.Combo.Hits} Hit Combo";
                }

                return;
            }

            m_DisplayTimer += Time.deltaTime;
            if (m_DisplayTimer >= timer)
            {
                attackText.text = "";
                comboText.text = "";
            }
        }
    }
}