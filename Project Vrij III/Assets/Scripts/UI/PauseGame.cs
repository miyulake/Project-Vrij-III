using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private InputReader input;

    private void Start() 
    {
        input = GetComponent<InputReader>();
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }

    private void Update()
    {
        if (input.Pause) TogglePause();
    }

    private void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }
}