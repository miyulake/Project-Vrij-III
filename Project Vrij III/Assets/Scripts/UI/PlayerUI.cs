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

        public void UpdateUI(EntityManager manager, float timer)
        {
            if (manager.Entity.RecievedComboHits == 1) // Reset on first hit and set after
            {
                attackText.text = "";
                comboText.text = "";
            }

            if (manager.State.CurrentState == EntityState.HITSTUN)
            {
                m_DisplayTimer = 0;

                if (manager.Entity.HitType == ContactType.COUNTER) attackText.text = "Counter";
                else if (manager.Entity.HitType == ContactType.PUNISH) attackText.text = "Punish";

                if (manager.Entity.RecievedComboHits > 1)
                    comboText.text = $"{manager.Entity.RecievedComboHits} Hit Combo\n{manager.Entity.RecievedComboDamage} Damage";

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