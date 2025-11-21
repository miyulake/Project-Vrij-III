using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool MatchEnded { get; private set; } = false;

    [Header("Game Mode")]
    public bool usePaint = true;

    [Header("Events")]
    [SerializeField] private UnityEvent m_OnPaintMatchEnd;
    [SerializeField] private UnityEvent m_OnNormalMatchEnd;

    [Header("Game Settings")]
    [SerializeField] private int m_FrameRate = 60;

    [Header("Match Settings")]
    public int maxHealth = 100;
    [SerializeField] private TextMeshProUGUI m_TimerTextMesh;
    [SerializeField] private int m_MatchTime = 60;
    private float m_MatchTimer;

    private void Awake() 
    {
        Instance = this;
        Time.fixedDeltaTime = 1f / m_FrameRate;
    }

    private void Start()
    {
        Application.targetFrameRate = m_FrameRate;
        StartMatch();
    }

    private void Update() => HandleMatchTimer();

    private void HandleMatchTimer()
    {
        if (MatchEnded) return;

        m_MatchTimer -= Time.deltaTime;
        if (m_MatchTimer <= 0f)
        {
            m_MatchTimer = 0f;
            EndMatch();
        }
        m_TimerTextMesh.text = m_MatchTimer.ToString("00");
    }

    public void SetMatchState(bool matchState) => MatchEnded = matchState;

    public void StartMatch()
    {
        MatchEnded = false;
        m_MatchTimer = m_MatchTime;
        m_TimerTextMesh.text = m_MatchTimer.ToString("00");
    }

    public void EndMatch()
    {
        if (usePaint)
        {
            m_OnPaintMatchEnd.Invoke();
            PaintManager.Instance.GetCoverageResult();
        }
        else m_OnNormalMatchEnd.Invoke();
        MatchEnded = true;
    }

    public bool IsPaused() => Time.timeScale < 0.99f;
}