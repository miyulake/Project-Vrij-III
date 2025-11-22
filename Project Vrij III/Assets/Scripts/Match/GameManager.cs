using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Settings")]
    [SerializeField] private int m_FrameRate = 60;

    [Header("Mode Settings")]
    public bool usePaint = true;
    public int maxHealth = 100;

    private void Awake() 
    {
        Instance = this;
        Time.fixedDeltaTime = 1f / m_FrameRate;
    }

    private void Start() => Application.targetFrameRate = m_FrameRate;
    
    public bool IsPaused() => Time.timeScale < 0.99f;
}