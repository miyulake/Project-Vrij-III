public abstract class EntityContext
{
    protected Entity Entity { get; private set; }
    protected void SetEntity(Entity entity) => Entity = entity;

    // MonoBehaviours
    protected TwoDMovement MovementComp => Entity.Get<TwoDMovement>();
    protected AttackHandler AttackComp => Entity.Get<AttackHandler>();
    protected SuperHandler SuperComp => Entity.Get<SuperHandler>();
    protected ThrowHandler ThrowComp => Entity.Get<ThrowHandler>();
    protected TauntHandler TauntComp => Entity.Get<TauntHandler>();
    protected EntityView ViewComp => Entity.Get<EntityView>();
    protected EntityEffects EffectsComp => Entity.Get<EntityEffects>();
    protected EntityVisuals VisualsComp => Entity.Get<EntityVisuals>();
    protected EntityVFX VFXComp => Entity.Get<EntityVFX>();
    protected EntityOrientation OrientationComp => Entity.Get<EntityOrientation>();
    protected EntityAnimator AnimatorComp => Entity.Get<EntityAnimator>();
    protected EntityAudio AudioComp => Entity.Get<EntityAudio>();
    protected ShakeController ShakeComp => Entity.Get<ShakeController>();
    protected InputReader InputComp => Entity.Get<InputReader>();

    // Non-mono
    protected StateMachine StateMachine => Entity.Get<StateMachine>();
    protected EntityResolver ResolverComp => Entity.Get<EntityResolver>();
    protected EntityResources ResourcesComp => Entity.Get<EntityResources>();
    protected EntityPhysics PhysicsComp => Entity.Get<EntityPhysics>();
    protected ComboTracker ComboComp => Entity.Get<ComboTracker>();

    // Opponent
    protected Entity Opponent => Entity.Opponent;
}
