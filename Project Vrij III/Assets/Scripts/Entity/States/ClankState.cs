public class ClankState : EntityState
{
    public ClankState(Entity entity) : base(entity) { }

    public override void OnEnter()
    {
        Combo.Reset();
        Resolver.ResolveHit(Throw.GetClank());
        Orientation.ForceFixOrientation();
    }
}
