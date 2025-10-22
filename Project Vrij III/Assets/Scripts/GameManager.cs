using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Match Settings")]
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private int matchTime = 60;
    public bool MatchHasEnded { get; private set; } = false;
    private float matchTimer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        matchTimer = matchTime;
        textMesh.text = matchTimer.ToString("0.00");
    }

    private void Update()
    {
        if (MatchHasEnded) return;

        matchTimer -= Time.deltaTime;
        if (matchTimer <= 0f)
        {
            matchTimer = 0f;
            MatchHasEnded = true;
        }
        textMesh.text = matchTimer.ToString("00");
    }

    public void SetMatchState(bool matchState) => MatchHasEnded = matchState;

    public void StartMatch()
    {
        MatchHasEnded = false;
        matchTimer = matchTime;
    }
}
