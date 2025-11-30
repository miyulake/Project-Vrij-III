public class IdleState : EntityState
{
    public IdleState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void OnEnter()
    {
        //
    }

    public override void Tick()
    {
        if (Entity.Input.Blocking)
        {
            StateMachine.ChangeState<BlockState>();
            return;
        }
    }

    public override void OnExit()
    {
        //
    }
}
