using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI p1Text, p2Text;
    [SerializeField] private Slider p1Slider, p2Slider;
    [SerializeField] private AnimationCurve resultCurve;
    [SerializeField] private float resultCurveDuration = 1f;
    private float resultCurveTime = 0f;

    private void Update()
    {
        if (GameManager.Instance.MatchEnded) SetUI();
    }

    private void SetUI()
    {
        var paintManager = PaintManager.Instance;

        p1Text.text = $"{paintManager.Player1Percentage}%";
        p2Text.text = $"{paintManager.Player2Percentage}%";

        resultCurveTime += Time.deltaTime;
        var t = Mathf.Clamp01(resultCurveTime / resultCurveDuration);
        var curveValue = resultCurve.Evaluate(t);
        p1Slider.value = Mathf.Lerp(0, paintManager.Player1Percentage, curveValue);
        p2Slider.value = Mathf.Lerp(0, paintManager.Player2Percentage, curveValue);
    }
}
