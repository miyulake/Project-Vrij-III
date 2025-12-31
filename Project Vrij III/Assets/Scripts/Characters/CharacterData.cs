using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterSO/Data")]
public class CharacterData : ScriptableObject
{
    [Header("General")]
    public CharacterType characterType;

    [Header("Settings")]
    public CharacterSettings characterSettings;

    [Header("Moves")]
    public Moves allMoves;
    public Supers allSupers;

    public MovementSettings GetMovement() => characterSettings.movement;
    public EffectSettings GetEffects() => characterSettings.effects;
    public MoveData[] GetAllMoves() => allMoves.entries;
    public SuperData[] GetAllSupers() => allSupers.entries;
}
