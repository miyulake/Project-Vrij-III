public class HitStunState : EntityState
{
    private int m_StunFrames;

    public HitStunState(Entity entity, int stun) : base(entity) => m_StunFrames = stun;

    public override void OnEnter()
    {
        Animator.Play("Stun");
    }

    public override void Tick()
    {
        m_StunFrames--;

        if (m_StunFrames <= 0)
        {
            Combo.Reset();

            if (Input.Block)
                StateMachine.ChangeState<BlockState>();
            else
                StateMachine.ChangeState<IdleState>();
        }
    }
}
