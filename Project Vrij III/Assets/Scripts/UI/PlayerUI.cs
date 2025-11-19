using UnityEngine;
using TMPro;
using Coffee.UIEffects;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CombatUI m_PlayerOneUI;
    [SerializeField] private CombatUI m_PlayerTwoUI;
    [SerializeField] private float m_DisplayDuration = 1f;

    private void Update()
    {
        m_PlayerOneUI.HandleComboUI(m_DisplayDuration);
        m_PlayerTwoUI.HandleComboUI(m_DisplayDuration);
    }

    [System.Serializable]
    private class CombatUI
    {
        public StateManager opponentState;
        public Entity opponent;
        public TextMeshProUGUI attackText, comboText;
        public UIEffectTweener effectTweener;

        private bool m_InCombo;
        private float m_ComboTimer;

        public void HandleComboUI(float timer)
        {
            if (opponentState.CurrentState == EntityState.RECOVER) attackText.text = "Punish";
            else if (opponentState.CurrentState == EntityState.ATTACK) attackText.text = "Counter";

            if (opponentState.CurrentState == EntityState.HITSTUN && opponent.ComboHits > 1)
            {
                m_ComboTimer = 0;

                comboText.text =
                    $"{opponent.ComboHits} Hit Combo \n {opponent.ComboDamage} Damage";

                m_InCombo = true;
            }
            else m_InCombo = false;

            if (!m_InCombo)
            {
                m_ComboTimer += Time.deltaTime;
                if (m_ComboTimer >= timer)
                {
                    attackText.text = "";
                    comboText.text = "";
                }
            }
        }
    }
}
