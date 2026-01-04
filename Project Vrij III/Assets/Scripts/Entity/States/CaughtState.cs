public class CaughtState : EntityState
{
    private int m_BreakFrames;

    public CaughtState(Entity entity, int breakFrames) : base(entity) =>
        m_BreakFrames = breakFrames;

    public override void OnEnter()
    {
        AnimatorComp.PlayEnd("Stun");
        Entity.Pause();
        Opponent.Pause();
        AudioComp.GetPlay(SoundType.ThrowCaught);
    }

    public override void Tick()
    {
        if (Entity.Get<InputReader>().Grab)
        {
            AudioComp.GetPlay(SoundType.ThrowBreak);
            Opponent.Get<StateMachine>().ChangeState<ClankState>();
            StateMachine.ChangeState<ClankState>();
            return;
        }

        m_BreakFrames--;
        if (m_BreakFrames <= 0)
        {
            ResolverComp.ApplyStoredMove();
            Opponent.Get<EntityOrientation>().ManualTurn(true);
        }
    }

    public override void OnExit()
    {
        Entity.Resume();
        Opponent.Resume();
    }
}