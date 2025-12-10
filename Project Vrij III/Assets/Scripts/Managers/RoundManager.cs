using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    public RoundState CurrentState { get; private set; } = RoundState.INTRO;
    public int PlayerOneWins { get; private set; } = 0;
    public int PlayerTwoWins { get; private set; } = 0;

    [Header("Sequence Settings")]
    public float introDuration = 2f;
    public float knockoutDuration = 2f;
    public float resultDuration = 3f;

    [Header("Round Settings")]
    [Range(1, 5)][SerializeField] private int m_WinsNeeded = 3;
    [SerializeField] private TextMeshProUGUI m_TimerTextMesh;
    [SerializeField] private int m_RoundDuration = 60;
    private float m_RoundTimer;
    private int m_CurrentRound;

    private Coroutine m_RoundFlowRoutine;

    [Header("Events")]
    [SerializeField] private UnityEvent m_OnRoundStart;
    [SerializeField] private UnityEvent m_OnPaintRoundEnd;
    [SerializeField] private UnityEvent m_OnHealthRoundEnd;
    [SerializeField] private UnityEvent m_OnRoundEnd;

    private void Awake() => Instance = this;

    private void Start() => StartNewRound();

    private void StartNewRound()
    {
        ++m_CurrentRound;

        m_OnRoundStart.Invoke();

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

        EndRound();

        yield return new WaitForSeconds(resultDuration);
        // RESULT END

        StartNewRound();
    }

    private void EndRound()
    {
        if (GameManager.Instance.CurrentMode == GameMode.PAINT)
            m_OnPaintRoundEnd.Invoke();
        else
            m_OnHealthRoundEnd.Invoke();

        // We are updating the round win UI in this event (temp hack)
        if (PlayerOneWins != m_WinsNeeded && PlayerTwoWins != m_WinsNeeded) m_OnRoundEnd.Invoke();
    }

    private void SetSlowMo(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.0167f * timeScale;
    }

    public void AddRoundWin()
    {
        // Health result
        var playerOne = PlayerManager.Instance.playerOne;
        var playerTwo = PlayerManager.Instance.playerTwo;
        var playerOneHealth = playerOne.Health.CurrentHealth;
        var playerTwoHealth = playerTwo.Health.CurrentHealth;

        // Paint result
        var paintManager = PaintManager.Instance;
        var playerOneResult = paintManager.PlayerOnePercentage;
        var playerTwoResult = paintManager.PlayerTwoPercentage;

        if (playerOneResult > playerTwoResult || playerOneHealth > playerTwoHealth) ++PlayerOneWins;
        else if (playerTwoResult > playerOneResult || playerTwoHealth > playerOneHealth) ++PlayerTwoWins;
    }

    public int WinsNeeded() => m_WinsNeeded;
}
