using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private CombatUI m_PlayerOneUI;
    [SerializeField] private CombatUI m_PlayerTwoUI;

    private void Update()
    {
        m_PlayerOneUI.SetComboUI();
        m_PlayerTwoUI.SetComboUI();
    }

    [System.Serializable]
    private class CombatUI
    {
        public StateManager opponentState;
        public Entity opponent;
        public TextMeshProUGUI attackText, comboText;

        //public void SetAttackUI() => attackText.text = "";
        public void SetComboUI()
        {
            if (opponentState.CurrentState == EntityState.HITSTUN && opponent.ComboHits > 1)
            {
                comboText.text =
                    $"{opponent.ComboHits} Hit Combo \n {opponent.ComboDamage} Damage";
            }
            else comboText.text = "";
        }
    }
}
