using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public static PaintManager Instance;
    [SerializeField] private RenderTexture paintTexture;

    private void Awake() => Instance = this;

    public int GetWinner()
    {
        Texture2D tex = new Texture2D(paintTexture.width, paintTexture.height, TextureFormat.RGB24, false);

        RenderTexture.active = paintTexture;
        tex.ReadPixels(new Rect(0, 0, paintTexture.width, paintTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        Color32[] pixels = tex.GetPixels32();
        int p1Count = 0;
        int p2Count = 0;

        foreach (var px in pixels)
        {
            if (px.r > 200 && px.g < 50 && px.b < 50) p1Count++;
            else if (px.b > 200 && px.r < 50 && px.g < 50) p2Count++;
        }

        Debug.Log($"Player 1: {p1Count}, Player 2: {p2Count}");

        return (p1Count > p2Count) ? 1 : 2;
    }
}
