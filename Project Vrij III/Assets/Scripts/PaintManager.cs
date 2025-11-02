using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public static PaintManager Instance;
    public int Player1Percentage { get; private set; }
    public int Player2Percentage { get; private set; }
    [SerializeField] private RenderTexture paintTexture;

    private void Awake() => Instance = this;

    public void GetCoverage()
    {
        var texture = new Texture2D(paintTexture.width, paintTexture.height, TextureFormat.RGB24, false);

        RenderTexture.active = paintTexture;
        texture.ReadPixels(new Rect(0, 0, paintTexture.width, paintTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        var pixels = texture.GetPixels32();
        var p1Count = 0;
        var p2Count = 0;

        foreach (var px in pixels)
        {
            if (px.r > px.g && px.r > px.b) p1Count++; // Red-dominant pixel
            else if (px.b > px.r && px.b > px.g) p2Count++; // Blue-dominant pixel
        }

        Player1Percentage = Mathf.RoundToInt(p1Count / (float)pixels.Length * 100f);
        Player2Percentage = Mathf.RoundToInt(p2Count / (float)pixels.Length * 100f);

        Debug.Log($"Player 1: {Player1Percentage}%, Player 2: {Player2Percentage}%");
    }

    public int GetWinner()
    {
        GetCoverage();
        return (Player1Percentage > Player2Percentage) ? 1 : 2;
    }
}
