using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public static PaintManager Instance { get; private set; }
    public string WinText { get; private set; }
    public int Player1Percentage { get; private set; }
    public int Player2Percentage { get; private set; }

    [SerializeField] private RenderTexture m_PaintTexture;
    [Range(32, 512)] [SerializeField] private int m_SampleSize = 128;
    private RenderTexture m_SmallTexture;
    private Texture2D m_PaintCopy;

    private void Awake() 
    {
        Instance = this;

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
        var p1Count = 0; var p2Count = 0;

        for (int i = 0; i < pixels.Length; i++)
        {
            var px = pixels[i];
            if      (px.r > px.g && px.r > px.b) p1Count++;
            else if (px.b > px.r && px.b > px.g) p2Count++;
        }

        Player1Percentage = Mathf.RoundToInt(p1Count / (float)pixels.Length * 100f);
        Player2Percentage = Mathf.RoundToInt(p2Count / (float)pixels.Length * 100f);

        WinText = 
            (Player1Percentage > Player2Percentage) ? "Red Wins!"  :
            (Player2Percentage > Player1Percentage) ? "Blue Wins!" :
            "Draw!";

        Debug.Log($"Player 1: {p1Count} pixels, Player 2: {p2Count} pixels");
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
