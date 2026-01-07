using UnityEngine;

[CreateAssetMenu(fileName = "Super", menuName = "CombatSO/Single Data/Super", order = 1)]
public class SuperData : MoveData
{
    [Header("Super")]
    public SuperType superType;
    public int meterCost;
    public int freezeFrames;
    public int activationFrames;

    public override bool HasEnoughMeter(EntityResources resources) =>
        resources.Meter.Current >= meterCost;
}
