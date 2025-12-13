using UnityEngine;
using UnityEngine.UI;

public class UIAlphaHandler : MonoBehaviour
{
    [SerializeField] private RectTransform m_TargetRect;
    [SerializeField] private Image[] m_TargetImages;
    [SerializeField] private float m_FadedAlpha = 0.3f;
    [SerializeField] private float m_FadeSpeed = 5f;
    private float m_TargetAlpha = 1f;

    private void Update() => HandleAlpha();

    private void HandleAlpha()
    {
        var overlappingUI = false;

        Transform[] players = { PlayerManager.Instance.playerOne.transform, PlayerManager.Instance.playerTwo.transform };
        for (int i = 0; i < players.Length; i++)
        {
            var screenPos = Camera.main.WorldToScreenPoint(players[i].position);
            if (RectTransformUtility.RectangleContainsScreenPoint(m_TargetRect, screenPos))
            {
                overlappingUI = true;
                break;
            }
        }
        
        m_TargetAlpha = overlappingUI ? m_FadedAlpha : 1f;
        var currentAlpha = m_TargetImages[0].color.a;
        var newAlpha = Mathf.Lerp(currentAlpha, m_TargetAlpha, Time.deltaTime * m_FadeSpeed);
        for (int i = 0; i < m_TargetImages.Length; i++)
        {
            var color = m_TargetImages[i].color;
            color.a = newAlpha;
            m_TargetImages[i].color = color;
        }
    }
}
