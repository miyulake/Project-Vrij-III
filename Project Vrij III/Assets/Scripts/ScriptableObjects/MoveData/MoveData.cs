using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "CombatSO/Move")]
public class MoveData : ScriptableObject
{
    public string moveId;
    [Space]
    public int startupFrames;
    public int activeFrames;
    public int recoveryFrames;
    [Space]
    public bool isCancelable;
    public MoveData[] cancelOptions;
    [Space]
    public bool hasArmor;
    public bool counterHitBonus;
}
