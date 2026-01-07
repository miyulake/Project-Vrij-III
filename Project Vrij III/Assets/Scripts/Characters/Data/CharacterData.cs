using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "CharacterSO/New Character", order = 0)]
public class CharacterData : ScriptableObject
{
    [Header("General")]
    public CharacterType characterType;

    [Header("Universal")]
    public CombatSettings combat;
    public OrientationSettings orientation;
    [SerializeField] private Moves m_GenericMoves;
    public MoveData[] AllGenerics => m_GenericMoves.entries;

    [Header("Unique")]
    [SerializeField] private CharacterSettings m_CharacterSettings;
    public MovementSettings Movement => m_CharacterSettings.movement;
    public EffectSettings Effects    => m_CharacterSettings.effects;
    public VisualsSettings Visuals   => m_CharacterSettings.visuals;
    public PaintSettings Paint       => m_CharacterSettings.paint;

    [Header("Moves")]
    [SerializeField] private Moves m_AllMoves;
    [SerializeField] private Supers m_AllSupers;
    public MoveData[] AllMoves   => m_AllMoves.entries;
    public SuperData[] AllSupers => m_AllSupers.entries;

    [Header("Audio")]
    public AudioData audio;
}
