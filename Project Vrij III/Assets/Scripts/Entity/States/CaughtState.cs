public class CaughtState : EntityState
{
    private int m_BreakFrames;

    public CaughtState(Entity entity, StateMachine stateMachine, int breakFrames) : base(entity, stateMachine) =>
        m_BreakFrames = breakFrames;

    public override void OnEnter()
    {
        Entity.Orientation.StoreOrientation();
    }

    public override void Tick()
    {
        if (Entity.Input.Grab)
        {
            Entity.Opponent.StateMachine.ChangeState<ClankState>();
            StateMachine.ChangeState<ClankState>();
            return;
        }

        m_BreakFrames--;
        if (m_BreakFrames <= 0)
        {
            Entity.Resolver.isForced = true;
            Entity.Resolver.ResolveHit(Entity.Resolver.StoredMove);
        }
    }
}