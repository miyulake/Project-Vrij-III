using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public static PaintManager Instance;
    [SerializeField] private RenderTexture paintTexture;

    private void Awake() => Instance = this;

    public void GetCoverage(out int p1Percentage, out int p2Percentage)
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

        p1Percentage = Mathf.RoundToInt(p1Count / (float)pixels.Length * 100f);
        p2Percentage = Mathf.RoundToInt(p2Count / (float)pixels.Length * 100f);

        Debug.Log($"Player 1: {p1Percentage}%, Player 2: {p2Percentage}%");
    }

    public int GetWinner()
    {
        GetCoverage(out int p1Percentage, out int p2Percentage);
        return (p1Percentage > p2Percentage) ? 1 : 2;
    }
}
