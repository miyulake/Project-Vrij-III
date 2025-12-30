public class IdleState : EntityState
{
    public IdleState(Entity entity) : base(entity) { }

    public override void Tick()
    {
        if (InputComp.Block && RoundManager.Instance.CurrentState != RoundState.INTRO)
        {
            StateMachine.ChangeState<BlockState>();
            return;
        }
    }
}
