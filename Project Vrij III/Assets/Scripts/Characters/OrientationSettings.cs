using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "CharacterSO/Universal/Orientation Settings", order = 0)]
public class OrientationSettings : ScriptableObject
{
    public AnimationCurve turnCurve;
    [Range(0, 1)]
    public float turnDuration = 0.2f;
}
