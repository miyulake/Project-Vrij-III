using NUnit.Framework.Internal;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    public RoundState CurrentState { get; private set; } = RoundState.INTRO;
    public int PlayerOneWins { get; private set; } = 0;
    public int PlayerTwoWins { get; private set; } = 0;

    [Header("Sequence Settings")]
    public float introDuration = 2f;
    public float knockoutDuration = 1f;
    public float timeUpDuration = 2f;
    public float resultDuration = 2f;

    [Header("Round Settings")]
    [Range(1, 5)][SerializeField] private int m_WinsNeeded = 3;
    [SerializeField] private TextMeshProUGUI m_TimerTextMesh;
    [SerializeField] private int m_RoundDuration = 60;
    private RoundWinner m_RoundWinner;
    private float m_RoundTimer;
    private int m_CurrentRound;

    private Coroutine m_RoundFlowRoutine;

    [Header("Events")]
    [SerializeField] private UnityEvent m_OnMatchEnd;

    private void Awake() => Instance = this;

    private void Start() => StartRound();

    private void StartRound()
    {
        ++m_CurrentRound;

        PaintManager.Instance.ClearPaintBackground();
        PaintResultUI.Instance.ResetPaintResult();
        PlayerManager.Instance.playerOne.ResetEntity();
        PlayerManager.Instance.playerTwo.ResetEntity();

        m_RoundTimer = m_RoundDuration;
        m_TimerTextMesh.text = m_RoundTimer.ToString("00");

        if (m_RoundFlowRoutine != null) StopCoroutine(m_RoundFlowRoutine);
        m_RoundFlowRoutine = StartCoroutine(RoundFlow());
    }

    public void SetState(RoundState newState)
    {
        CurrentState = newState;
        CameraController.Instance.ResetSetup();
    }

    private IEnumerator RoundFlow()
    {
        // INTRO START
        SetState(RoundState.INTRO);

        CameraController.Instance.SetStartSetup();
        CameraController.Instance.ResetSetup();

        if (IsFinalRound(PlayerOneWins, PlayerTwoWins, m_WinsNeeded))
            RoundUI.Instance.SetRoundText("Final Round");
        else
            RoundUI.Instance.SetRoundText($"Round {m_CurrentRound}");
        yield return new WaitForSeconds(introDuration);

        RoundUI.Instance.SetRoundText("Fight!");
        yield return new WaitForSeconds(introDuration / 2);
        // INTRO END

        // GAMEPLAY START
        SetState(RoundState.GAMEPLAY);

        while (CurrentState == RoundState.GAMEPLAY && m_RoundTimer > 0f)
        {
            m_RoundTimer -= Time.deltaTime;
            m_TimerTextMesh.text = Mathf.CeilToInt(m_RoundTimer).ToString("00");
            yield return null;
        }
        // GAMEPLAY END

        // KNOCKOUT START (skip if we use paint or round timer didn't end)
        if (GameManager.Instance.CurrentMode != GameMode.PAINT && m_RoundTimer > 0f)
        {
            if (CurrentState != RoundState.KNOCKOUT) SetState(RoundState.KNOCKOUT);

            SetSlowMo(0.1f);

            RoundUI.Instance.SetRoundText("K.O.");
            yield return new WaitForSecondsRealtime(knockoutDuration); // Realtime

            SetSlowMo(1);
        }
        // KNOCKOUT END

        // RESULT START
        SetState(RoundState.RESULT);

        GetPaintResult(); // Get paint result

        // TIME UP | DRAW
        m_RoundWinner = GetRoundWinner(); // Get winner
        if (m_RoundTimer <= 0)
        {
            RoundUI.Instance.SetRoundText("Time Up");

            if (m_RoundWinner == RoundWinner.DRAW)
            {
                yield return new WaitForSeconds(timeUpDuration);
                RoundUI.Instance.SetRoundText("Draw", false);
            }
        }

        EndRound();

        yield return new WaitForSeconds(resultDuration);
        // RESULT END

        // END MATCH | START NEW ROUND
        if (PlayerOneWins == m_WinsNeeded || PlayerTwoWins == m_WinsNeeded) EndMatch();
        else StartRound();
    }

    private void EndRound()
    {
        // Updating round win UI if no one won
        if (PlayerOneWins != m_WinsNeeded && PlayerTwoWins != m_WinsNeeded)
        {
            if (m_RoundWinner == RoundWinner.P1)
                ++PlayerOneWins;
            else if (m_RoundWinner == RoundWinner.P2)
                ++PlayerTwoWins;

            RoundTracker.Instance.UpdateRoundWinUI();
        }
    }

    public void StartMatch()
    {
        // Reset match variables etc...
        PlayerOneWins = 0;
        PlayerTwoWins = 0;
        m_CurrentRound = 0;
        RoundTracker.Instance.ResetRoundWinUI();
        StartRound();
    }

    private void EndMatch()
    {
        // Do end match stuff - end animation or something...
        RoundUI.Instance.SetRoundText(GetWinText(), false);
        m_OnMatchEnd.Invoke();
    }

    private void SetSlowMo(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.0167f * timeScale;
    }

    private void GetPaintResult()
    {
        PaintManager.Instance.GetCoverageResult();
        PaintResultUI.Instance.BeginPaintResult();
    }

    private RoundWinner GetRoundWinner()
    {
        var usingPaint = GameManager.Instance.CurrentMode == GameMode.PAINT;

        if (usingPaint)
        {
            var paintManager = PaintManager.Instance;
            var playerOneResult = paintManager.PlayerOnePercentage;
            var playerTwoResult = paintManager.PlayerTwoPercentage;

            if (playerOneResult > playerTwoResult) return RoundWinner.P1;
            if (playerTwoResult > playerOneResult) return RoundWinner.P2;
            return RoundWinner.DRAW;
        }
        else
        {
            var playerOne = PlayerManager.Instance.playerOne;
            var playerTwo = PlayerManager.Instance.playerTwo;
            var playerOneHealth = playerOne.Health.CurrentHealth;
            var playerTwoHealth = playerTwo.Health.CurrentHealth;

            if (playerOneHealth > playerTwoHealth) return RoundWinner.P1;
            if (playerTwoHealth > playerOneHealth) return RoundWinner.P2;
            return RoundWinner.DRAW;
        }
    }

    private bool IsFinalRound(int winsA, int winsB, int winsNeeded) =>
        winsA == winsNeeded - 1 && winsB == winsNeeded - 1;

    public int GetWinsNeeded() => m_WinsNeeded;

    public void SetWinsNeeded(int wins) => m_WinsNeeded = wins;

    private string GetWinText() => PlayerOneWins > PlayerTwoWins ? "Red Wins" : "Blue Wins";

    public int GetRoundDuration() => m_RoundDuration;

    public void SetRoundDuration(int duration) => m_RoundDuration = duration;
}
