public class DeadState : EntityState
{
    public DeadState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void OnEnter()
    {
        Entity.Visuals.SetDeadFace();
    }

    public override void Tick()
    {
        //
    }

    public override void OnExit()
    {
        
    }
}
