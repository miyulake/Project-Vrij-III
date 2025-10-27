using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int frameRate = 60;

    [Header("Match Settings")]
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private int matchTime = 60;
    public bool MatchEnded { get; private set; } = false;
    private float matchTimer;

    private void Start()
    {
        Application.targetFrameRate = frameRate;

        StartMatch();
        textMesh.text = matchTimer.ToString("0.00");
    }

    private void Update()
    {
        HandleMatchTimer();
    }

    private void HandleMatchTimer()
    {
        if (MatchEnded) return;

        matchTimer -= Time.deltaTime;
        if (matchTimer <= 0f)
        {
            matchTimer = 0f;
            MatchEnded = true;
        }
        textMesh.text = matchTimer.ToString("00");
    }

    public void SetMatchState(bool matchState) => MatchEnded = matchState;

    public void StartMatch()
    {
        MatchEnded = false;
        matchTimer = matchTime;
    }
}
