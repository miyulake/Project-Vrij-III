using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Winner;

    [Header("Normal Results")]
    [SerializeField] private StateManager m_PlayerOne;
    [SerializeField] private StateManager m_PlayerTwo;

    [Header("Paint Results")]
    [SerializeField] private TextMeshProUGUI p1Percent;
    [SerializeField] private TextMeshProUGUI p2Percent;
    [SerializeField] private Slider p1Slider, p2Slider;
    [SerializeField] private AnimationCurve resultCurve;
    [SerializeField] private float resultCurveDuration = 1f;
    private float resultCurveTime;
    private bool isAnimating = false;
    private bool hasRun = false;

    private void Update()
    {
        if (GameManager.Instance.usePaint)
        {
            if (GameManager.Instance.MatchEnded && !isAnimating && !hasRun) BeginPaintResult();
            if (isAnimating) AnimatePaintResult();
        }
    }

    public void DisplayNormalResult()
    {
        m_Winner.text = 
            (m_PlayerOne.CurrentState != EntityState.DEAD && m_PlayerTwo.CurrentState == EntityState.DEAD) ? "Red Wins!"  :
            (m_PlayerOne.CurrentState == EntityState.DEAD && m_PlayerTwo.CurrentState != EntityState.DEAD) ? "Blue Wins!" :
            "Draw";
    }

    private void BeginPaintResult()
    {
        hasRun = true;
        m_Winner.text = "";
        resultCurveTime = 0f;
        isAnimating = true;
    }

    private void AnimatePaintResult()
    {
        resultCurveTime += Time.deltaTime;
        var time = Mathf.Clamp01(resultCurveTime / resultCurveDuration);
        var curve = resultCurve.Evaluate(time);

        p1Slider.value = Mathf.Lerp(0, PaintManager.Instance.Player1Percentage, curve);
        p2Slider.value = Mathf.Lerp(0, PaintManager.Instance.Player2Percentage, curve);

        if (time >= 1f)
        {
            isAnimating = false;
            p1Percent.text = $"{PaintManager.Instance.Player1Percentage}%";
            p2Percent.text = $"{PaintManager.Instance.Player2Percentage}%";
            m_Winner.text = PaintManager.Instance.WinMessage;
        }
    }
}
