using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [Header("Paint Results")]
    [SerializeField] private TextMeshProUGUI playerOnePercent;
    [SerializeField] private TextMeshProUGUI playerTwoPercent;
    [SerializeField] private Slider playerOneSlider, playerTwoSlider;
    [SerializeField] private AnimationCurve resultCurve;
    [SerializeField] private float resultCurveDuration = 1f;
    private float startTime;
    private bool started = false;

    private void Update()
    {
        if (GameManager.Instance.CurrentMode != GameMode.PAINT || RoundManager.Instance.CurrentState != RoundState.RESULT) return;

        if (!started) BeginPaintResult();
        else AnimatePaintResult();
    }

    public void DisplayHealthResult()
    {
        var playerOne = PlayerManager.Instance.playerOne;   
        var playerTwo = PlayerManager.Instance.playerTwo;
        var playerOneHealth = playerOne.Health.CurrentHealth;
        var playerTwoHealth = playerTwo.Health.CurrentHealth;

        RoundUI.Instance.SetRoundText(
            (playerOneHealth > playerTwoHealth) ? "Red Wins!" :
            (playerTwoHealth > playerOneHealth) ? "Blue Wins!" :
            "Draw", false);
    }

    public void ResetPaintResult() => started = false;

    private void BeginPaintResult()
    {
        started = true;
        startTime = Time.time;
        playerOneSlider.value = 0;
        playerTwoSlider.value = 0;
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

        if (time >= 1f) RoundUI.Instance.SetRoundText(paintManager.WinMessage, false);
    }
}
