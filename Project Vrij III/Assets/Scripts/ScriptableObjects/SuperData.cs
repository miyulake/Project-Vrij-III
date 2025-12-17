using UnityEngine;

[CreateAssetMenu(fileName = "Super", menuName = "CombatSO/Super", order = 1)]
public class SuperData : MoveData
{
    [Header("Super")]
    public SuperType superType;
    public int meterCost;
    public FrameRange freezeFrames;

    public override bool CanExecute(Entity entity) =>
        entity.Resources.CurrentMeter >= meterCost;
}
