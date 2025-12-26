public class SuperState : EntityState
{
    private int m_FreezeFrames;
    private int m_ActivationFrames;

    public SuperState(Entity entity, int freezeFrames, int activationFrames) : base(entity) 
    {
        m_ActivationFrames = activationFrames;
        m_FreezeFrames = freezeFrames;
    }

    public override void Tick()
    {
        // TO-DO: SlowMo + Darkened background
        RoundManager.Instance.SetSlowMo(0.1f);

        m_FreezeFrames--;
        if (m_FreezeFrames <= 0)
        {
            RoundManager.Instance.SetSlowMo(1);

            m_ActivationFrames--;
            if (m_ActivationFrames <= 0)
            {
                if (Input.Block)
                    StateMachine.ChangeState<BlockState>();
                else
                    StateMachine.ChangeState<IdleState>();
            }
        }
    }

    public override void OnExit()
    {
        Super.ExitSuper(Super.GetSuperData());
    }
}
