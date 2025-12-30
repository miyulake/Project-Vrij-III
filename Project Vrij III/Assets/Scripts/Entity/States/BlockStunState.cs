public class BlockStunState : EntityState
{
    private int m_StunFrames;

    public BlockStunState(Entity entity, int stun) : base(entity) =>
        m_StunFrames = stun;

    public override void OnEnter()
    {
        AnimatorComp.Play("Block_Stun");
    }

    public override void Tick()
    {
        m_StunFrames--;

        if (m_StunFrames <= 0)
        {
            if (InputComp.Block)
                StateMachine.ChangeState<BlockState>();
            else
                StateMachine.ChangeState<IdleState>();
        }
    }
}
