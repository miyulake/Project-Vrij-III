using UnityEngine;

public static class PaintRegister
{
    private static readonly Paint[] m_PaintObjects = new Paint[512];
    private static int m_Count;

    public static void Register(Paint paint)
    {
        if (m_Count >= m_PaintObjects.Length)
        {
            Debug.LogError("Paint register overflow!");
            return;
        }
        m_PaintObjects[m_Count++] = paint;
    }

    public static void ClearAll()
    {
        for (int i = 0; i < m_Count; i++) Object.Destroy(m_PaintObjects[i].gameObject);
        m_Count = 0;
    }
}