public class BlockState : EntityState
{
    public BlockState(Entity entity) : base(entity) { }

    public override void Tick()
    {
        if (!InputComp.Block)
        {
            StateMachine.ChangeState<IdleState>();
            return;
        }
    }
}
