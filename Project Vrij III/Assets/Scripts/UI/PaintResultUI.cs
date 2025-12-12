using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PaintResultUI : MonoBehaviour
{
    public static PaintResultUI Instance { get; private set; }
    [SerializeField] private GameObject m_PaintResult;
    [SerializeField] private TextMeshProUGUI playerOnePercent;
    [SerializeField] private TextMeshProUGUI playerTwoPercent;
    [SerializeField] private Slider playerOneSlider, playerTwoSlider;
    [SerializeField] private AnimationCurve resultCurve;
    [SerializeField] private float resultCurveDuration = 1f;
    private float startTime;
    private bool started = false;

    private void Awake() => Instance = this;

    private void Update()
    {
        if (GameManager.Instance.CurrentMode != GameMode.PAINT || 
            RoundManager.Instance.CurrentState != RoundState.RESULT) return;

        AnimatePaintResult();
    }

    public void ResetPaintResult()
    {
        m_PaintResult.SetActive(false);
        started = false;
    }

    public void BeginPaintResult()
    {
        PaintManager.Instance.GetCoverageResult();
        m_PaintResult.SetActive(true);
        playerOneSlider.value = 0;
        playerTwoSlider.value = 0;
        startTime = Time.time;
        started = true;
    }

    private void AnimatePaintResult()
    {
        var time = Mathf.Clamp01((Time.time - startTime) / resultCurveDuration);
        var curve = resultCurve.Evaluate(time);

        var paintManager = PaintManager.Instance;
        var playerOneResult = paintManager.PlayerOnePercentage;
        var playerTwoResult = paintManager.PlayerTwoPercentage;

        playerOneSlider.value = playerOneResult * curve;
        playerTwoSlider.value = playerTwoResult * curve;

        playerOnePercent.text = $"{Mathf.RoundToInt(playerOneSlider.value)}%";
        playerTwoPercent.text = $"{Mathf.RoundToInt(playerTwoSlider.value)}%";
    }
}
