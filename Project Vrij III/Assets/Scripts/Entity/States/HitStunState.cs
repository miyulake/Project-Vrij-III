public class HitStunState : EntityState
{
    private int m_StunFrames;

    public HitStunState(Entity entity, int stun) : base(entity) => m_StunFrames = stun;

    public override void OnEnter()
    {
        AnimatorComp.Play("Stun");
    }

    public override void Tick()
    {
        m_StunFrames--;

        if (m_StunFrames <= 0)
        {
            ComboComp.Reset();

            if (InputComp.Block)
                StateMachine.ChangeState<BlockState>();
            else
                StateMachine.ChangeState<IdleState>();
        }
    }
}
