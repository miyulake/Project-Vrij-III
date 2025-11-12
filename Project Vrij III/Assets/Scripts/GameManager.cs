using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool MatchEnded { get; private set; } = false;
    [SerializeField] private UnityEvent m_OnMatchEnd;
    private StateManager[] m_Entities;

    [Header("Game Settings")]
    [SerializeField] private int m_FrameRate = 60;

    [Header("Match Settings")]
    [SerializeField] private TextMeshProUGUI m_TextMesh;
    [SerializeField] private int m_MatchTime = 60;
    private float m_MatchTimer;

    private void Awake() => Instance = this;

    private void Start()
    {
        Application.targetFrameRate = m_FrameRate;
        StartMatch();
        m_TextMesh.text = m_MatchTimer.ToString("0.00");
        m_Entities = FindObjectsByType<StateManager>(FindObjectsSortMode.None);
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
        m_TextMesh.text = m_MatchTimer.ToString("00");
    }

    public void SetMatchState(bool matchState) => MatchEnded = matchState;

    public void StartMatch()
    {
        MatchEnded = false;
        m_MatchTimer = m_MatchTime;
    }

    public void EndMatch()
    {
        m_OnMatchEnd.Invoke();
        PaintManager.Instance.GetCoverageResult();
        MatchEnded = true;
    }

    public void KillEntities()
    {
        for (int i = 0; i < m_Entities.Length; i++)
            m_Entities[i].SetState(EntityState.DEAD);
    }
}
