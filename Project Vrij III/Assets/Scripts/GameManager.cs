using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool MatchEnded { get; private set; } = false;
    [SerializeField] private UnityEvent onMatchEnd;

    [Header("Game Settings")]
    [SerializeField] private int frameRate = 60;

    [Header("Match Settings")]
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private int matchTime = 60;
    private float matchTimer;

    private void Awake() => Instance = this;

    private void Start()
    {
        Application.targetFrameRate = frameRate;

        StartMatch();
        textMesh.text = matchTimer.ToString("0.00");
    }

    private void Update() => HandleMatchTimer();

    private void HandleMatchTimer()
    {
        if (MatchEnded) return;

        matchTimer -= Time.deltaTime;
        if (matchTimer <= 0f)
        {
            matchTimer = 0f;
            EndMatch();
        }
        textMesh.text = matchTimer.ToString("00");
    }

    public void SetMatchState(bool matchState) => MatchEnded = matchState;

    public void StartMatch()
    {
        MatchEnded = false;
        matchTimer = matchTime;
    }

    public void EndMatch()
    {
        onMatchEnd.Invoke();
        PaintManager.Instance.GetWinner(); // Double calling it :(
        MatchEnded = true;
    }
}
