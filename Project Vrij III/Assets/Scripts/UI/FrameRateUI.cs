using UnityEngine;
using TMPro;

public class FrameRateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI framerateTextMesh;
    [SerializeField] private float updateInterval = 0.5f;
    private float accumulatedTime = 0f;
    private float averageFrameRate = 0f;
    private int framesCounted = 0;

    private void Update() => FrameRateDisplay();

    private void FrameRateDisplay()
    {
        accumulatedTime += Time.unscaledDeltaTime;
        framesCounted++;

        if (accumulatedTime >= updateInterval)
        {
            averageFrameRate = framesCounted / accumulatedTime;
            framerateTextMesh.text = $"{Mathf.RoundToInt(averageFrameRate)} fps";

            accumulatedTime = 0f;
            framesCounted = 0;
        }
    }
}
