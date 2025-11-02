using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI p1Text, p2Text;
    [SerializeField] private Slider p1Slider, p2Slider;
    [SerializeField] private AnimationCurve resultCurve;
    [SerializeField] private float curveDuration = 1f;

    private void Update()
    {
        if (GameManager.Instance.MatchEnded) SetUI();
    }

    private void SetUI()
    {
        var paintManager = PaintManager.Instance;

        p1Text.text = $"{paintManager.Player1Percentage}%";
        p1Slider.value = paintManager.Player1Percentage;

        p2Text.text = $"{paintManager.Player2Percentage}%";
        p2Slider.value = paintManager.Player2Percentage;
    }
}
