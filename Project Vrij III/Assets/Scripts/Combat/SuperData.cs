using UnityEngine;

[CreateAssetMenu(fileName = "Super", menuName = "CombatSO/SingleData/Super", order = 1)]
public class SuperData : MoveData
{
    [Header("Super")]
    public SuperType superType;
    public int meterCost;
    public int freezeFrames;
    public int activationFrames;

    public override bool CanExecute(EntityResources resources) =>
        resources.Meter.Current >= meterCost;
}
