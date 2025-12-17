public class SuperState : EntityState
{
    public SuperState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void OnEnter()
    {
        //Entity.Throw.GetPlayerCollider().enabled = false;
    }

    public override void Tick()
    {
        //
    }

    public override void OnExit()
    {
        //Entity.Throw.GetPlayerCollider().enabled = true;
    }
}
