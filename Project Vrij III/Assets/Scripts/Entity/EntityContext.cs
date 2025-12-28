public abstract class EntityContext
{
    protected Entity Entity { get; private set; }
    protected void SetEntity(Entity entity) => Entity = entity;

    // Components - MonoBehaviours
    protected TwoDMovement Movement => Entity.Get<TwoDMovement>();
    protected AttackHandler Attack => Entity.Get<AttackHandler>();
    protected SuperHandler Super => Entity.Get<SuperHandler>();
    protected ThrowHandler Throw => Entity.Get<ThrowHandler>();
    protected TauntHandler Taunt => Entity.Get<TauntHandler>();

    // Specific - MonoBehaviours
    protected EntityVisuals Visuals => Entity.Get<EntityVisuals>();
    protected EntityVFX VFX => Entity.Get<EntityVFX>();
    protected EntityOrientation Orientation => Entity.Get<EntityOrientation>();
    protected EntityAnimator Animator => Entity.Get<EntityAnimator>();
    protected EntityAudio Audio => Entity.Get<EntityAudio>();

    // Specific - Non-mono
    protected StateMachine StateMachine => Entity.Get<StateMachine>();
    protected EntityResolver Resolver => Entity.Get<EntityResolver>();
    protected EntityResources Resources => Entity.Get<EntityResources>();
    protected EntityPhysics Physics => Entity.Get<EntityPhysics>();
    protected ComboTracker Combo => Entity.Get<ComboTracker>();

    // Misc - MonoBehaviours
    protected ShakeController Shake => Entity.Get<ShakeController>();
    protected InputReader Input => Entity.Get<InputReader>();

    // Opponent
    protected Entity Opponent => Entity.Opponent;
}
