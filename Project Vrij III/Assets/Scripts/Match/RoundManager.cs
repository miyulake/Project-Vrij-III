using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    public RoundState CurrentState { get; private set; } = RoundState.INTRO;
    public bool RoundEnded { get; private set; } = false;

    [SerializeField] private float m_IntroDuration = 3f;
    private float m_StateTimer = 0f;

    [Header("Round Settings")]
    [SerializeField] private TextMeshProUGUI m_TimerTextMesh;
    [SerializeField] private int m_RoundDuration = 60;
    private float m_RoundTimer;

    [Header("Events")]
    [SerializeField] private UnityEvent m_OnPaintRoundEnd;
    [SerializeField] private UnityEvent m_OnNormalRoundEnd;

    private void Awake() => Instance = this;

    private void Start() => StartRound();

    private void Update()
    {
        HandleRoundState();
        HandleRoundTimer();
    }

    private void SetState(RoundState newState)
    {
        CurrentState = newState;
        m_StateTimer = 0f;

        CameraController.Instance.ResetSetup();
    }

    private void HandleRoundState()
    {
        m_StateTimer += Time.deltaTime;

        switch (CurrentState)
        {
            case RoundState.INTRO:
                if (m_StateTimer >= m_IntroDuration) SetState(RoundState.GAMEPLAY);
                break;

            case RoundState.GAMEPLAY:

                break;

            case RoundState.KNOCKOUT:

                break;

            case RoundState.RESULT:

                break;
        }
    }

    private void StartRound()
    {
        RoundEnded = false;
        m_RoundTimer = m_RoundDuration;
        m_TimerTextMesh.text = m_RoundTimer.ToString("00");

        CameraController.Instance.SetStartSetup();

        SetState(RoundState.INTRO); 
    }

    public void EndRound()
    {
        if (GameManager.Instance.usePaint)
        {
            m_OnPaintRoundEnd.Invoke();
            PaintManager.Instance.GetCoverageResult();
        }
        else m_OnNormalRoundEnd.Invoke();

        RoundEnded = true;

        SetState(RoundState.RESULT);
    }

    private void HandleRoundTimer()
    {
        if (RoundEnded) return;

        m_RoundTimer -= Time.deltaTime;
        if (m_RoundTimer <= 0f)
        {
            m_RoundTimer = 0f;
            EndRound();
        }
        m_TimerTextMesh.text = m_RoundTimer.ToString("00");
    }
}
