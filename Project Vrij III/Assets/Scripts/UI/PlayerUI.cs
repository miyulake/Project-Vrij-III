using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CombatUI m_PlayerOneUI;
    [SerializeField] private CombatUI m_PlayerTwoUI;
    [SerializeField] private float m_DisplayDuration = 1f;

    private void Update()
    {
        m_PlayerOneUI.UpdateUI(PlayerManager.Instance.playerTwo.State, PlayerManager.Instance.playerTwo.Entity, 
            m_DisplayDuration);
        m_PlayerTwoUI.UpdateUI(PlayerManager.Instance.playerOne.State, PlayerManager.Instance.playerOne.Entity, 
            m_DisplayDuration);
    }

    [System.Serializable]
    private class CombatUI
    {
        public TextMeshProUGUI attackText, comboText;
        private float m_DisplayTimer;

        public void UpdateUI(StateManager opponentState, Entity opponent, float timer)
        {
            if (opponentState.CurrentState == EntityState.HITSTUN)
            {
                m_DisplayTimer = 0;

                if (opponent.HitType == ContactType.COUNTER) attackText.text = "Counter";
                else if (opponent.HitType == ContactType.PUNISH) attackText.text = "Punish";

                if (opponent.RecievedComboHits > 1)
                    comboText.text = $"{opponent.RecievedComboHits} Hit Combo\n{opponent.RecievedComboDamage} Damage";

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