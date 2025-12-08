using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    [SerializeField] private int m_FrameRate = 60;

    [Header("Mode Settings")]
    public GameMode CurrentMode { get; private set; }
    public int maxHealth = 100;

    [Header("Events")]
    [SerializeField] private UnityEvent m_OnEnterHealthMode;
    [SerializeField] private UnityEvent m_OnEnterPaintMode;
    [SerializeField] private UnityEvent m_OnEnterPongMode;

    private void Awake() 
    {
        Instance = this;
        Time.fixedDeltaTime = 1f / m_FrameRate;
    }

    private void Start()
    {
        Application.targetFrameRate = m_FrameRate;
        SetGameMode(0); // Start default game mode
    }
    
    public bool IsPaused() => Time.timeScale < 0.99f;

    public void SetGameMode(int index) 
    {
        CurrentMode = (GameMode)index;

        switch (CurrentMode)
        {
            case GameMode.HEALTH:
                m_OnEnterHealthMode.Invoke();
                break;

            case GameMode.PAINT:
                m_OnEnterPaintMode.Invoke();
                break;

            case GameMode.PONG:
                m_OnEnterPongMode.Invoke();
                break;
        }
    }
}