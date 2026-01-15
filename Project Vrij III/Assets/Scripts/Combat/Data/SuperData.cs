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

    public bool CanActivate(Entity entity)
    {
        var resources = entity.Get<EntityResources>();
        var @throw = entity.Get<ThrowHandler>();
        var stateMachine = entity.Get<StateMachine>();
        if (@throw == null || resources == null) return false;

        if (HasEnoughMeter(resources))
        {
            return superType switch
            {
                SuperType.NONE => false,
                SuperType.CHAIN => @throw.GrabConnected,
                SuperType.TIME => false,
                SuperType.TAUNT => stateMachine.IsNeutral(),
                _ => false,
            };
        }
        else return false;
    }
}