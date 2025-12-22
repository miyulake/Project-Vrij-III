using Game.Entities;
using UnityEngine;

public abstract class EntityComponent : MonoBehaviour, IEntityComponent
{
    protected Entity Entity { get; private set; }
    public virtual void Initialize(Entity entity) => Entity = entity;

    // Components
    protected StateMachine StateMachine => Entity.Get<StateMachine>();
    protected TwoDMovement MovementComp => Entity.Get<TwoDMovement>();

    // Handlers
    protected AttackHandler AttackComp => Entity.Get<AttackHandler>();
    protected SuperHandler SuperComp => Entity.Get<SuperHandler>();
    protected ThrowHandler ThrowComp => Entity.Get<ThrowHandler>();
    protected TauntHandler TauntComp => Entity.Get<TauntHandler>();

    // Specific
    protected EntityResources ResourcesComp => Entity.Get<EntityResources>();
    protected EntityResolver ResolverComp => Entity.Get<EntityResolver>();
    protected EntityVisuals VisualsComp => Entity.Get<EntityVisuals>();
    protected EntityVFX VFXComp => Entity.Get<EntityVFX>();
    protected EntityOrientation OrientationComp => Entity.Get<EntityOrientation>();
    protected EntityAnimator AnimatorComp => Entity.Get<EntityAnimator>();
    protected EntityAudio AudioComp => Entity.Get<EntityAudio>();

    // Created
    protected EntityPhysics PhysicsComp => Entity.Physics;
    protected ComboTracker ComboTracker => Entity.Combo;

    // Misc
    protected ShakeController ShakeComp => Entity.Get<ShakeController>();
    protected InputReader Input => Entity.Get<InputReader>();

    // Opponent
    protected Entity Opponent => Entity.Opponent;
}
