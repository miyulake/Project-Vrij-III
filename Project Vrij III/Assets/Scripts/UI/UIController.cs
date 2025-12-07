using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private InputReader[] m_Readers;

    private void Awake() => m_Readers = FindObjectsByType<InputReader>(FindObjectsSortMode.None);

    private void Start() => Time.timeScale = pauseMenu.activeSelf ? 0 : 1;

    private void Update()
    {
        if (RoundManager.Instance.CurrentState == RoundState.KNOCKOUT) return;

        for (int i = 0; i < m_Readers.Length; i++)
        {
            if (m_Readers[i].Restart) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else if (m_Readers[i].Pause) TogglePause();
        }
    }

    private void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }
}