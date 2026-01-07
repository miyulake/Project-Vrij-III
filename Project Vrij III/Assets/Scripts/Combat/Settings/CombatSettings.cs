using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "CombatSO/Universal/Combat Settings", order = 0)]
public class CombatSettings : ScriptableObject
{
    [Header("Buffer")]
    [Range(0, 1)]
    public float bufferCrossfade = 0.1f;
    [Range(0, 10)]
    public int bufferFrames = 10;

    [Header("Effects")]
    [Range(0, -5)]
    public float particleZPosition = -3;
}
