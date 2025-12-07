public class ClankState : EntityState
{
    public ClankState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void OnEnter()
    {
        Entity.Combo.Reset();
        Entity.Resolver.ResolveHit(Entity.Throw.GetClank());
        Entity.Orientation.ForceFixOrientation();
    }
}
