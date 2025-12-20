public class BlockState : EntityState
{
    public BlockState(Entity entity) : base(entity) { }

    public override void Tick()
    {
        if (!Input.Block)
        {
            StateMachine.ChangeState<IdleState>();
            return;
        }
    }
}
