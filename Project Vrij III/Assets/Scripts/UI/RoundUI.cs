using UnityEngine;
using TMPro;

public class RoundUI : MonoBehaviour
{
    public static RoundUI Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI m_RoundTextMesh;
    [SerializeField] private float m_DisplayDuration = 1;
    private bool m_IsDisplaying = false;
    private float m_DisplayTime = 0f;

    private void Awake() => Instance = this;

    private void Update() => HandleDisplay();

    public void SetRoundText(string newText, bool useTimer = true)
    {
        m_RoundTextMesh.text = newText;
        m_RoundTextMesh.gameObject.SetActive(true);

        if (useTimer)
        {
            m_DisplayTime = 0f;
            m_IsDisplaying = true;
        }
    }

    private void HandleDisplay()
    {
        if (m_IsDisplaying)
        {
            m_DisplayTime += Time.deltaTime;
            if (m_DisplayTime >= m_DisplayDuration)
            {
                m_RoundTextMesh.text = "";
                m_RoundTextMesh.gameObject.SetActive(false);

                m_DisplayTime = 0f;
                m_IsDisplaying = false;
            }
        }
    }
}
