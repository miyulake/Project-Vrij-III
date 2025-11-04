using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winner;
    [SerializeField] private TextMeshProUGUI p1Percent, p2Percent;
    [SerializeField] private Slider p1Slider, p2Slider;
    [SerializeField] private AnimationCurve resultCurve;
    [SerializeField] private float resultCurveDuration = 1f;

    private float resultCurveTime;
    private bool isAnimating = false;
    private bool hasRun = false;

    private void Update()
    {
        if (GameManager.Instance.MatchEnded && !isAnimating && !hasRun) BeginResult();
        if (isAnimating) AnimateResult();
    }

    private void BeginResult()
    {
        hasRun = true;

        p1Percent.text = $"{PaintManager.Instance.Player1Percentage}%";
        p2Percent.text = $"{PaintManager.Instance.Player2Percentage}%";
        winner.text = "";

        resultCurveTime = 0f;
        isAnimating = true;
    }

    private void AnimateResult()
    {
        resultCurveTime += Time.deltaTime;
        var time = Mathf.Clamp01(resultCurveTime / resultCurveDuration);
        var curve = resultCurve.Evaluate(time);

        p1Slider.value = Mathf.Lerp(0, PaintManager.Instance.Player1Percentage, curve);
        p2Slider.value = Mathf.Lerp(0, PaintManager.Instance.Player2Percentage, curve);

        if (time >= 1f)
        {
            isAnimating = false;
            winner.text = $"Player {PaintManager.Instance.GetWinner()} Wins!";
        }
    }
}
