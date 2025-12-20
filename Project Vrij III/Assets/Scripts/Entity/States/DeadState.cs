public class DeadState : EntityState
{
    public DeadState(Entity entity) : base(entity) { }

    public override void OnEnter()
    {
        Taunt.Reset();
        Visuals.SetDeadFace();
        Animator.PlayEnd("Stun");
        Entity.Pause();
    }

    public override void OnExit()
    {
        Entity.Resume();
    }
}
