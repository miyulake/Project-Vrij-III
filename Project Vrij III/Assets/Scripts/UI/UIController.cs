using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;

        var entities = PlayerManager.Instance.All;
        for (int i = 0; i < entities.Count; i++)
            entities[i].Get<InputReader>().PauseEvent += TogglePause;
    }

    private void OnDisable()
    {
        var entities = PlayerManager.Instance.All;
        for (int i = 0; i < entities.Count; i++)
            entities[i].Get<InputReader>().PauseEvent -= TogglePause;
    }

    public void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Time.timeScale = pauseMenu.activeSelf ? 0 : 1;
    }
}