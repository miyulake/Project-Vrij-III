using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public static PaintManager Instance { get; private set; }
    public int PlayerOnePercentage { get; private set; }
    public int PlayerTwoPercentage { get; private set; }

    [SerializeField] private RenderTexture m_PaintTexture;
    [Range(32, 512)] [SerializeField] private int m_SampleSize = 128;
    [SerializeField] private float m_PaintZ = 3;

    private RenderTexture m_SmallTexture;
    private Texture2D m_PaintCopy;
    private Color32 m_P1Color, m_P2Color;

    public float PaintZ => m_PaintZ;

    private void Awake() 
    {
        Instance = this;
        new PaintRegister();

        m_P1Color = PlayerManager.Instance.characterOne.Paint.color;
        m_P2Color = PlayerManager.Instance.characterTwo.Paint.color;

        var adjustedAspect = m_PaintTexture.width / m_PaintTexture.height;
        m_SmallTexture = new RenderTexture(m_SampleSize, m_SampleSize / adjustedAspect, 0, RenderTextureFormat.ARGB32)
        {
            filterMode = FilterMode.Point
        };
        m_PaintCopy = new Texture2D(m_SampleSize, m_SampleSize / adjustedAspect, TextureFormat.RGB24, false);
    } 

    public void GetCoverageResult()
    {
        Graphics.Blit(m_PaintTexture, m_SmallTexture);
        RenderTexture.active = m_SmallTexture;
        m_PaintCopy.ReadPixels(new Rect(0, 0, m_SampleSize, m_SmallTexture.height), 0, 0);
        m_PaintCopy.Apply(false);
        RenderTexture.active = null;

        var pixels = m_PaintCopy.GetPixels32();
        var p1Count = 0; 
        var p2Count = 0;

        for (int i = 0; i < pixels.Length; i++)
        {
            var d1 = ColorDistance(pixels[i], m_P1Color);
            var d2 = ColorDistance(pixels[i], m_P2Color);
            if      (d1 < d2) p1Count++;
            else if (d2 < d1) p2Count++;
        }

        PlayerOnePercentage = Mathf.RoundToInt(p1Count / (float)pixels.Length * 100f);
        PlayerTwoPercentage = Mathf.RoundToInt(p2Count / (float)pixels.Length * 100f);

        Debug.Log($"Player 1: {p1Count} pixels, Player 2: {p2Count} pixels");
    }

    private int ColorDistance(Color32 a, Color32 b)
    {
        var dr = a.r - b.r;
        var dg = a.g - b.g;
        var db = a.b - b.b;
        return dr * dr + dg * dg + db * db;
    }

    private void OnDestroy()
    {
        if (m_SmallTexture != null) m_SmallTexture.Release();
    }

    // For debugging purposes
    /*
    private void OnGUI()
    {
        if (m_SmallTexture != null)
            GUI.DrawTexture(new Rect(75, 10, 128, 128), m_SmallTexture, ScaleMode.ScaleToFit, false);
    }
    */
}
