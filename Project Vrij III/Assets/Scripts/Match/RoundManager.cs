using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    public RoundState CurrentState { get; private set; } = RoundState.INTRO;
    private float m_StateTimer = 0f;

    [Header("Sequence Settings")]
    public float introDuration = 2f;
    public float knockoutDuration = 2f;

    [Header("Round Settings")]
    [Range(1, 5)] [SerializeField] private int m_RoundsNeededToWin = 3;
    [SerializeField] private TextMeshProUGUI m_TimerTextMesh;
    [SerializeField] private int m_RoundDuration = 60;
    private float m_RoundTimer;

    [Header("Events")]
    [SerializeField] private UnityEvent m_OnPaintRoundEnd;
    [SerializeField] private UnityEvent m_OnNormalRoundEnd;

    private void Awake() => Instance = this;

    private void Start() => StartRound();

    private void Update() => HandleRoundState();

    public void SetState(RoundState newState)
    {
        CurrentState = newState;
        m_StateTimer = 0f;

        CameraController.Instance.ResetSetup();
    }

    private void HandleRoundState()
    {
        if (CurrentState != RoundState.RESULT) m_StateTimer += Time.deltaTime;

        switch (CurrentState)
        {
            case RoundState.INTRO:
                if (m_StateTimer >= introDuration) SetState(RoundState.GAMEPLAY);
                break;

            case RoundState.GAMEPLAY:
                HandleRoundTimer();
                // DO GAMEPLAY LOGIC
                break;

            case RoundState.KNOCKOUT:
                //if (GameManager.Instance.usePaint) SetState(RoundState.RESULT); // Instant result when using paint
                if (m_StateTimer >= knockoutDuration) SetState(RoundState.RESULT);
                break;

            case RoundState.RESULT:
                GetMatchResult();
                break;
        }
    }

    public void StartRound()
    {
        m_RoundTimer = m_RoundDuration;
        m_TimerTextMesh.text = m_RoundTimer.ToString("00");

        CameraController.Instance.SetStartSetup();

        SetState(RoundState.INTRO);
    }

    private void GetMatchResult()
    {
        if (GameManager.Instance.usePaint)
        {
            m_OnPaintRoundEnd.Invoke();
            PaintManager.Instance.GetCoverageResult();
        }
        else m_OnNormalRoundEnd.Invoke();
    }

    private void HandleRoundTimer()
    {
        m_RoundTimer -= Time.deltaTime;
        if (m_RoundTimer <= 0f)
        {
            m_RoundTimer = 0f;
            // Round timed out so end it
            SetState(RoundState.KNOCKOUT);
        }
        m_TimerTextMesh.text = Mathf.CeilToInt(m_RoundTimer).ToString("00");
    }
}
