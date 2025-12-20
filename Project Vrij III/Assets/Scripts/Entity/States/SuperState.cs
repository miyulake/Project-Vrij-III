public class SuperState : EntityState
{
    public SuperState(Entity entity) : base(entity) { }

    public override void OnEnter()
    {
        // Invincible = true
    }

    public override void Tick()
    {
        // SlowMo + Animation + Darkened background
    }

    public override void OnExit()
    {
        // Invincible = false
    }
}
