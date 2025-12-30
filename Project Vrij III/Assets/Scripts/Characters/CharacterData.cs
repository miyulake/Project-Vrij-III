using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterSO/Data")]
public class CharacterData : ScriptableObject
{
    public CharacterType characterType;
    public MoveData[] moves;
}
