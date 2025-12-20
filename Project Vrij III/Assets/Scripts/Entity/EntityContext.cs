public abstract class EntityContext
{
    protected Entity Entity { get; private set; }
    protected void SetEntity(Entity entity) => Entity = entity;

    // Components
    protected StateMachine StateMachine => Entity.Get<StateMachine>();
    protected TwoDMovement Movement => Entity.Get<TwoDMovement>();

    // Handlers
    protected AttackHandler Attack => Entity.Get<AttackHandler>();
    protected SuperHandler Super => Entity.Get<SuperHandler>();
    protected ThrowHandler Throw => Entity.Get<ThrowHandler>();
    protected TauntHandler Taunt => Entity.Get<TauntHandler>();

    // Specific
    protected EntityResources Resources => Entity.Get<EntityResources>();
    protected EntityResolver Resolver => Entity.Get<EntityResolver>();
    protected EntityVisuals Visuals => Entity.Get<EntityVisuals>();
    protected EntityVFX VFX => Entity.Get<EntityVFX>();
    protected EntityOrientation Orientation => Entity.Get<EntityOrientation>();
    protected EntityAnimator Animator => Entity.Get<EntityAnimator>();
    protected EntityAudio Audio => Entity.Get<EntityAudio>();

    // Created
    protected EntityPhysics Physics => Entity.Physics;
    protected ComboTracker Combo => Entity.Combo;

    // Misc
    protected ShakeController Shake => Entity.Get<ShakeController>();
    protected InputReader Input => Entity.Get<InputReader>();

    // Opponent
    protected Entity Opponent => Entity.Opponent;
}
