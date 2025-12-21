public class DeadState : EntityState
{
    public DeadState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine) { }

    public override void OnEnter()
    {
        Entity.Taunt.Reset();
        Entity.Visuals.SetDeadFace();
        Entity.Animator.PlayEnd("Stun");
        Entity.Pause(true);
    }

    public override void Tick()
    {
        //
    }

    public override void OnExit()
    {
        Entity.Pause(false);
    }
}
