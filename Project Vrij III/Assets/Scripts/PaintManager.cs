using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public static PaintManager Instance;

    public string WinText { get; private set; }
    public int Player1Percentage { get; private set; }
    public int Player2Percentage { get; private set; }

    [SerializeField] private RenderTexture paintTexture;
    [Range(32, 512)] [SerializeField] private int sampleSize = 64;
    private RenderTexture smallTexture;
    private Texture2D paintCopy;

    private void Awake() 
    {
        Instance = this;

        var adjustedAspect = paintTexture.width / paintTexture.height;
        smallTexture = new RenderTexture(sampleSize, sampleSize / adjustedAspect, 0, RenderTextureFormat.ARGB32)
        {
            filterMode = FilterMode.Point
        };
        paintCopy = new Texture2D(sampleSize, sampleSize / adjustedAspect, TextureFormat.RGB24, false);
    } 

    public void GetCoverageResult()
    {
        Graphics.Blit(paintTexture, smallTexture);
        RenderTexture.active = smallTexture;
        paintCopy.ReadPixels(new Rect(0, 0, sampleSize, smallTexture.height), 0, 0);
        paintCopy.Apply(false);
        RenderTexture.active = null;

        var pixels = paintCopy.GetPixels32();
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
        if (smallTexture != null) smallTexture.Release();
    }

    // For debugging purposes
    /*
    private void OnGUI()
    {
        if (smallTexture != null)
            GUI.DrawTexture(new Rect(75, 10, 128, 128), smallTexture, ScaleMode.ScaleToFit, false);
    }
    */
}
