public class HitStunState : EntityState
{
    private int m_StunFrames;

    public HitStunState(Entity entity, StateMachine stateMachine, int stun) : base(entity, stateMachine) =>
        m_StunFrames = stun;

    public override void OnEnter()
    {
        //
    }

    public override void Tick()
    {
        m_StunFrames--;

        if (m_StunFrames <= 0)
        {
            Entity.Combo.Reset();

            if (Entity.Input.Blocking)
                StateMachine.ChangeState<BlockState>();
            else
                StateMachine.ChangeState<IdleState>();
        }
    }

    public override void OnExit()
    {
        //
    }
}
