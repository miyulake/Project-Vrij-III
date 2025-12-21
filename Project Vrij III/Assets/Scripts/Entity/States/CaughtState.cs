public class CaughtState : EntityState
{
    private int m_BreakFrames;

    public CaughtState(Entity entity, StateMachine stateMachine, int breakFrames) : base(entity, stateMachine) =>
        m_BreakFrames = breakFrames;

    public override void OnEnter()
    {
        Entity.Animator.PlayEnd("Stun");
        Entity.Pause(true);
        Entity.Opponent.Pause(true);
        Entity.Audio.Play(Entity.Throw.GetCaughtSound());
    }

    public override void Tick()
    {
        if (Entity.Input.Grab)
        {
            Entity.Audio.Play(Entity.Throw.GetClankSound());
            Entity.Opponent.StateMachine.ChangeState<ClankState>();
            StateMachine.ChangeState<ClankState>();
            return;
        }

        m_BreakFrames--;
        if (m_BreakFrames <= 0)
        {
            Entity.Resolver.SetForceState(true);
            Entity.Resolver.ResolveHit(Entity.Resolver.StoredMove);
        }
    }

    public override void OnExit()
    {
        Entity.Pause(false);
        Entity.Opponent.Pause(false);
    }
}