using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "CombatSO/Move")]
public class MoveData : ScriptableObject
{
    [Header("General")]
    public MoveType moveType;
    public MoveFlags moveFlags;

    [Header("Frame Data")]
    public FrameData frames;

    [Header("Contact Data")]
    public ContactData hit;
    public ContactData block;
    public ContactData counterHit;

    [Header("Cancels")]
    public MoveData[] cancelOptions;

    [Header("Audio Visual")]
    public PaintData paintData;
    public EffectData effectData;
    public AnimationClip animationClip;
}
