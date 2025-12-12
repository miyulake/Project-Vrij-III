using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private InputReader[] m_Readers;

    private void Awake() => m_Readers = FindObjectsByType<InputReader>(FindObjectsSortMode.None);

    private void Start() => Time.timeScale = pauseMenu.activeSelf ? 0 : 1;

    private void Update()
    {
        // This is for debug, but should be != RoundState.GAMEPLAY
        if (RoundManager.Instance.CurrentState == RoundState.KNOCKOUT) return;

        for (int i = 0; i < m_Readers.Length; i++)
        {
            if (m_Readers[i].Pause) TogglePause();
        }
    }

    public void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }
}