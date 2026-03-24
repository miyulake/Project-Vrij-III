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
    [SerializeField] private float m_TimeUpDuration = 2f;
    [SerializeField] private float m_HealthResultDuration = 2f;
    [SerializeField] private float m_PaintResultDuration = 3f;

    [Header("Round Settings")]
    [Range(1, 5)][SerializeField] private int m_WinsNeeded = 3;
    [SerializeField] private TextMeshProUGUI m_TimerTextMesh;
    [SerializeField] private int m_RoundDuration = 60;
    [SerializeField] private Color m_LowTimeColor = Color.red;
    private RoundWinner m_RoundWinner;
    private float m_RoundTimer;
    private int m_CurrentRound;

    [Header("Events")]
    [SerializeField] private UnityEvent m_OnMatchEnd;

    [Header("Audio")]
    [SerializeField] private AudioSource m_AudioSource;
    // Announcer
    [SerializeField] private AudioClip m_FightSound;
    [SerializeField] private AudioClip m_KOSound;
    [SerializeField] private AudioClip m_PerfectSound;
    // UI
    [SerializeField] private AudioClip m_LowTimeSound;

    private Coroutine m_RoundFlowRoutine;

    private void Awake() => Instance = this;

    private void Start() => StartRound();

    private void StartRound()
    {
        ++m_CurrentRound;

        PaintRegister.Instance.ClearAll();
        PaintResultUI.Instance.ResetPaintResult();

        PlayerManager.Instance.playerOne.Reset();
        PlayerManager.Instance.playerTwo.Reset();

        m_RoundTimer = m_RoundDuration;
        m_TimerTextMesh.text = IsInfiniteTime() ? "∞" : m_RoundTimer.ToString("00");
        m_TimerTextMesh.color = Color.white;

        if (m_RoundFlowRoutine != null) StopCoroutine(m_RoundFlowRoutine);
        m_RoundFlowRoutine = StartCoroutine(RoundFlow());
    }

    public void SetState(RoundState newState)
    {
        CurrentState = newState;
        CameraController.Instance.ResetSetup();
    }

    // TO-DO: Create a global round flow manager, this is becoming too big
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

        RoundUI.Instance.SetRoundText("Fight");
        m_AudioSource.PlayOneShot(m_FightSound);
        yield return new WaitForSeconds(introDuration / 2);
        // INTRO END

        // GAMEPLAY START
        SetState(RoundState.GAMEPLAY);

        while (CurrentState == RoundState.GAMEPLAY)
        {
            if (!IsInfiniteTime())
            {
                if (m_RoundTimer <= 0f) break;

                m_RoundTimer -= Time.deltaTime;
                m_TimerTextMesh.text = Mathf.CeilToInt(m_RoundTimer).ToString("00");
                if (m_RoundTimer <= 10f) m_TimerTextMesh.color = m_LowTimeColor;
            }
            yield return null;
        }
        // GAMEPLAY END

        // KNOCKOUT START (skip if we use paint or round timer didn't end)
        if (GameManager.Instance.CurrentMode != GameMode.PAINT && m_RoundTimer > 0f)
        {
            if (CurrentState != RoundState.KNOCKOUT) SetState(RoundState.KNOCKOUT);

            if (IsPerfectKO())
            {
                RoundUI.Instance.SetRoundText("Perfect");
                m_AudioSource.PlayOneShot(m_PerfectSound);
            }
            else
            {
                RoundUI.Instance.SetRoundText("K.O.");
                m_AudioSource.PlayOneShot(m_KOSound);
            }

            SetSlowMo(0.1f);
            yield return new WaitForSecondsRealtime(knockoutDuration);

            SetSlowMo(1);
        }
        // KNOCKOUT END

        // RESULT START
        SetState(RoundState.RESULT);

        if (GameManager.Instance.CurrentMode == GameMode.PAINT) GetPaintResult(); // Get paint result

        // TIME UP | DRAW
        m_RoundWinner = GetRoundWinner();

        if (m_RoundTimer <= 0f)
        {
            RoundUI.Instance.SetRoundText("Time Up");

            if (m_RoundWinner == RoundWinner.DRAW)
            {
                yield return new WaitForSeconds(m_TimeUpDuration);
                RoundUI.Instance.SetRoundText("Draw", false);
            }
        }

        EndRound();

        yield return new WaitForSeconds(GameManager.Instance.CurrentMode == GameMode.PAINT ?
            m_PaintResultDuration : m_HealthResultDuration);
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
        MusicManager.Instance.SetRandomMusic(); // FOR PLAYTEST
        StartRound();
    }

    private void EndMatch()
    {
        // Do end match stuff - end animation or something...
        RoundUI.Instance.SetRoundText(GetWinText(), false);
        m_OnMatchEnd.Invoke();
    }

    public void SetSlowMo(float timeScale)
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
            var playerOneHealth = playerOne.Get<EntityResources>().Health.Current;
            var playerTwoHealth = playerTwo.Get<EntityResources>().Health.Current;

            if (playerOneHealth > playerTwoHealth) return RoundWinner.P1;
            if (playerTwoHealth > playerOneHealth) return RoundWinner.P2;
            return RoundWinner.DRAW;
        }
    }

    private bool IsFinalRound(int winsA, int winsB, int winsNeeded) =>
        winsA == winsNeeded - 1 && winsB == winsNeeded - 1;

    private bool IsPerfectKO()
    {
        var playerOne = PlayerManager.Instance.playerOne;
        var playerTwo = PlayerManager.Instance.playerTwo;
        var playerOneHealth = playerOne.Get<EntityResources>().Health;
        var playerTwoHealth = playerTwo.Get<EntityResources>().Health;
        return
            playerOneHealth.Current == playerOneHealth.Max ||
            playerTwoHealth.Current == playerTwoHealth.Max;
    }

    private bool IsInfiniteTime() => m_RoundTimer > 99;

    public int GetWinsNeeded() => m_WinsNeeded;

    public void SetWinsNeeded(int wins) => m_WinsNeeded = wins;

    private string GetWinText() => PlayerOneWins > PlayerTwoWins ? "Red Wins" : "Blue Wins";

    public int GetRoundDuration() => m_RoundDuration;

    public void SetRoundDuration(int duration) => m_RoundDuration = duration;
}
