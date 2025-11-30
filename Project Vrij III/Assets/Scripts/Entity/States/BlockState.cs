public class BlockState : EntityState
{
    public BlockState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void OnEnter()
    {
        //
    }

    public override void Tick()
    {
        if (!Entity.Input.Blocking)
        {
            StateMachine.ChangeState<IdleState>();
            return;
        }
    }

    public override void OnExit()
    {
        //
    }
}
