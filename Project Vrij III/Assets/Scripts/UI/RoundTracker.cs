using UnityEngine;
using UnityEngine.UI;

public class RoundTracker : MonoBehaviour
{
    [SerializeField] private Toggle[] m_PlayerOneRounds;
    [SerializeField] private Toggle[] m_PlayerTwoRounds;

    private void Start() => InitializeRoundWinUI();

    private void InitializeRoundWinUI()
    {
        var winsNeeded = RoundManager.Instance.WinsNeeded();

        for (int i = 0; i < winsNeeded; i++)
        {
            m_PlayerOneRounds[i].gameObject.SetActive(true);
            m_PlayerTwoRounds[i].gameObject.SetActive(true);
        }
    }

    public void UpdateRoundWinUI()
    {
        var playerOneWins = RoundManager.Instance.PlayerOneWins;
        var playerTwoWins = RoundManager.Instance.PlayerTwoWins;

        for (int i = 0; playerOneWins > 0 && i < m_PlayerOneRounds.Length; i++)
            m_PlayerOneRounds[playerOneWins - 1].isOn = true;

        for (int i = 0; playerTwoWins > 0 && i < m_PlayerTwoRounds.Length; i++)
            m_PlayerTwoRounds[playerTwoWins - 1].isOn = true;
    }

    public void ResetRoundWinUI()
    {
        // Both arrays are always the same size
        for (int i = 0; i < m_PlayerOneRounds.Length; i++)
        {
            m_PlayerOneRounds[i].isOn = false;
            //m_PlayerOneRounds[i].gameObject.SetActive(false);

            m_PlayerTwoRounds[i].isOn = false;
            //m_PlayerTwoRounds[i].gameObject.SetActive(false);
        }
    }
}
