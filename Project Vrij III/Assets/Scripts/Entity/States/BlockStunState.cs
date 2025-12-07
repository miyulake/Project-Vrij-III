public class BlockStunState : EntityState
{
    private int m_StunFrames;

    public BlockStunState(Entity entity, StateMachine stateMachine, int stun) : base(entity, stateMachine) =>
        m_StunFrames = stun;

    public override void OnEnter()
    {
        Entity.Animator.Play("Block_Stun");
    }

    public override void Tick()
    {
        m_StunFrames--;

        if (m_StunFrames <= 0)
        {
            if (Entity.Input.Block)
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
