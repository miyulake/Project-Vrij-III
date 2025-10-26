using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Match Settings")]
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private int matchTime = 60;
    public bool MatchEnded { get; private set; } = false;
    private float matchTimer;

    private void Start()
    {
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
