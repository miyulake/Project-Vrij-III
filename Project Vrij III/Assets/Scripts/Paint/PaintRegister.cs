using UnityEngine;

public class PaintRegister
{
    public static PaintRegister Instance { get; private set; }
    private readonly Paint[] m_PaintObjects = new Paint[512];
    private int m_Count;

    public PaintRegister() { Instance = this; }

    public void Register(Paint paint)
    {
        if (m_Count >= m_PaintObjects.Length)
        {
            Debug.LogError("Paint register overflow!");
            return;
        }
        m_PaintObjects[m_Count++] = paint;
    }

    public void ClearAll()
    {
        for (int i = 0; i < m_Count; i++) Object.Destroy(m_PaintObjects[i].gameObject);
        m_Count = 0;
    }
}