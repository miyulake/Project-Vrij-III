using Game.Entities;
using UnityEngine;

public abstract class EntityComponent : MonoBehaviour, IEntityComponent
{
    protected Entity Entity { get; private set; }
    public virtual void Initialize(Entity entity) => Entity = entity;

    // Components - MonoBehaviours
    protected TwoDMovement MovementComp => Entity.Get<TwoDMovement>();
    protected AttackHandler AttackComp => Entity.Get<AttackHandler>();
    protected SuperHandler SuperComp => Entity.Get<SuperHandler>();
    protected ThrowHandler ThrowComp => Entity.Get<ThrowHandler>();
    protected TauntHandler TauntComp => Entity.Get<TauntHandler>();

    // Specific - MonoBehaviours
    protected EntityEffects EffectsComp => Entity.Get<EntityEffects>();
    protected EntityVisuals VisualsComp => Entity.Get<EntityVisuals>();
    protected EntityVFX VFXComp => Entity.Get<EntityVFX>();
    protected EntityOrientation OrientationComp => Entity.Get<EntityOrientation>();
    protected EntityAnimator AnimatorComp => Entity.Get<EntityAnimator>();
    protected EntityAudio AudioComp => Entity.Get<EntityAudio>();

    // Specific - Non-mono
    protected StateMachine StateMachine => Entity.Get<StateMachine>();
    protected EntityResolver ResolverComp => Entity.Get<EntityResolver>();
    protected EntityResources ResourcesComp => Entity.Get<EntityResources>();
    protected EntityPhysics PhysicsComp => Entity.Get<EntityPhysics>();
    protected ComboTracker ComboComp => Entity.Get<ComboTracker>();

    // Misc - MonoBehaviours
    protected ShakeController ShakeComp => Entity.Get<ShakeController>();
    protected InputReader InputComp => Entity.Get<InputReader>();

    // Opponent
    protected Entity Opponent => Entity.Opponent;
}
