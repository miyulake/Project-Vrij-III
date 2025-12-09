using UnityEngine;

public class MoveListUI : MonoBehaviour
{
    [SerializeField] private GameObject m_ControllerInputs;
    [SerializeField] private GameObject m_KeyboardInputs;

    public void ToggleInputDisplay()
    {
        if (m_ControllerInputs.activeSelf)
        {
            m_KeyboardInputs.SetActive(true);
            m_ControllerInputs.SetActive(false);
        }
        else if (m_KeyboardInputs.activeSelf)
        {
            m_ControllerInputs.SetActive(true);
            m_KeyboardInputs.SetActive(false);
        }
    }
}
