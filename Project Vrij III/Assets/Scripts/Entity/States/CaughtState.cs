public class CaughtState : EntityState
{
    private int m_BreakFrames;

    public CaughtState(Entity entity, int breakFrames) : base(entity) =>
        m_BreakFrames = breakFrames;

    public override void OnEnter()
    {
        Animator.PlayEnd("Stun");
        Entity.Pause();
        Opponent.Pause();
        Audio.Play(Throw.GetCaughtSound());
    }

    public override void Tick()
    {
        if (Entity.Get<InputReader>().Grab)
        {
            Audio.Play(Throw.GetClankSound());
            Opponent.Get<StateMachine>().ChangeState<ClankState>();
            StateMachine.ChangeState<ClankState>();
            return;
        }

        m_BreakFrames--;
        if (m_BreakFrames <= 0)
        {
            Resolver.SetForceState(true);
            Resolver.ResolveHit(Resolver.StoredMove);
        }
    }

    public override void OnExit()
    {
        Entity.Resume();
        Opponent.Resume();
    }
}