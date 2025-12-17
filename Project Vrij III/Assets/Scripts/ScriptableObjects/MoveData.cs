using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "CombatSO/Move", order = 0)]
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

    [Header("Cancels")]
    public CancelOption[] cancelOptions;

    [Header("Paint")]
    public PaintData paintData;

    [Header("Active Hitboxes")]
    public int[] hitboxIndices;

    [Header("Throw")]
    public int breakFrames;

    [Header("Animation")]
    public string animationName;

    public virtual bool CanExecute(Entity entity) => true;
}

[System.Serializable]
public struct CancelOption
{
    public MoveData move;
    public float crossfadeDuration;
}