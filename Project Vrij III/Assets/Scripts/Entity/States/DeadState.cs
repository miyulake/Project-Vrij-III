public class DeadState : EntityState
{
    public DeadState(Entity entity) : base(entity) { }

    public override void OnEnter()
    {
        TauntComp.Reset();
        VisualsComp.ApplyDeadVisuals();
        AnimatorComp.PlayEnd("Stun");
        Entity.Pause();
    }

    public override void OnExit()
    {
        Entity.Resume();
    }
}
