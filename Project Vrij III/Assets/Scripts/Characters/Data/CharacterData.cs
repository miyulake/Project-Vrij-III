using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "CharacterSO/New Character", order = 0)]
public class CharacterData : ScriptableObject
{
    [Header("General")]
    public CharacterType characterType;

    [Header("Settings")]
    public CombatSettings combat;
    public OrientationSettings orientation;

    [SerializeField] private CharacterSettings m_CharacterSettings;

    public MovementSettings Movement => m_CharacterSettings.movement;
    public EffectSettings Effects    => m_CharacterSettings.effects;
    public MoveData[] AllMoves       => allMoves.entries;
    public SuperData[] AllSupers     => allSupers.entries;

    [Header("Moves")]
    public Moves allMoves;
    public Supers allSupers;

    [Header("Audio")]
    public AudioData audio;
}
