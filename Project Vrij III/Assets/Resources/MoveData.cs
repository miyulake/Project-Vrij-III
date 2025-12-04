using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "CombatSO/Move")]
public class MoveData : ScriptableObject
{
    [Header("General")]
    public MoveType moveType;
    public MoveFlags moveFlags;
    public AttackInput input;
    public bool startFromIdle;

    [Header("Frame Data")]
    public FrameData frames;

    [Header("Contact Data")]
    public ContactData hit;
    public ContactData block;
    public ContactData counterHit;

    [Header("Throw")]
    public int breakFrames;

    [Header("Cancels")]
    public CancelOption[] cancelOptions;

    [Header("Paint")]
    public PaintData paintData;

    [Header("Active Hitboxes")]
    public int[] hitboxIndices;

    [Header("Animation")]
    public string animationName;
}

[System.Serializable]
public struct CancelOption
{
    public MoveData move;
    public float crossfadeDuration;
}