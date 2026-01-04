public class ClankState : EntityState
{
    public ClankState(Entity entity) : base(entity) { }

    public override void OnEnter()
    {
        ComboComp.Reset();
        ResolverComp.ResolveHit(ThrowComp.Clank);
        OrientationComp.ForceFixOrientation();
    }
}
