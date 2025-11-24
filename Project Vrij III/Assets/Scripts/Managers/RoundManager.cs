using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    public RoundState CurrentState { get; private set; } = RoundState.INTRO;

    [Header("Sequence Settings")]
    public float introDuration = 2f;
    public float knockoutDuration = 2f;
    public float resultDuration = 3f;

    [Header("Round Settings")]
    [Range(1, 5)] [SerializeField] private int m_RoundsNeededToWin = 3;
    [SerializeField] private TextMeshProUGUI m_TimerTextMesh;
    [SerializeField] private int m_RoundDuration = 60;
    private float m_RoundTimer;
    private int m_CurrentRound;

    private Coroutine m_RoundFlowRoutine;

    [Header("Events")]
    [SerializeField] private UnityEvent m_OnPaintRoundEnd;
    [SerializeField] private UnityEvent m_OnNormalRoundEnd;
    //[SerializeField] private UnityEvent m_OnTimeRoundEnd;

    private void Awake() => Instance = this;

    private void Start() => StartNewRound();

    private void StartNewRound()
    {
        ++m_CurrentRound;

        PlayerManager.Instance.playerOne.Entity.ResetEntity();
        PlayerManager.Instance.playerTwo.Entity.ResetEntity();

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
        if (!GameManager.Instance.usePaint && m_RoundTimer > 0f)
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
        if (GameManager.Instance.usePaint)
        {
            m_OnPaintRoundEnd.Invoke();
            PaintManager.Instance.GetCoverageResult();
        }
        //else if (m_RoundTimer > 0) m_OnTimeRoundEnd.Invoke();
        else m_OnNormalRoundEnd.Invoke();
    }

    private void SetSlowMo(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.0167f * timeScale;
    }
}
