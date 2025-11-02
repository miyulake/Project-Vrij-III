using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI p1Text, p2Text;

    private void Update()
    {
        if (GameManager.Instance.MatchEnded) SetUI();
    }

    private void SetUI()
    {
        p1Text.text = $"{PaintManager.Instance.Player1Percentage}%";
        p2Text.text = $"{PaintManager.Instance.Player2Percentage}%";
    }
}
