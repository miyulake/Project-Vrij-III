using Game.Entities;
using UnityEngine;

public abstract class EntityComponent : MonoBehaviour, IEntityComponent
{
    protected Entity Entity { get; private set; }
    public virtual void Initialize(Entity entity) => Entity = entity;

    // MonoBehaviours
    protected SuperHandler SuperComp            => Entity.Get<SuperHandler>();
    protected EntityView ViewComp               => Entity.Get<EntityView>();

    // Non-mono
    protected StateMachine StateMachine         => Entity.Get<StateMachine>();
    protected TwoDMovement MovementComp         => Entity.Get<TwoDMovement>();
    protected AttackHandler AttackComp          => Entity.Get<AttackHandler>();
    protected TauntHandler TauntComp            => Entity.Get<TauntHandler>();
    protected ThrowHandler ThrowComp            => Entity.Get<ThrowHandler>();
    protected EntityResources ResourcesComp     => Entity.Get<EntityResources>();
    protected EntityResolver ResolverComp       => Entity.Get<EntityResolver>();
    protected EntityOrientation OrientationComp => Entity.Get<EntityOrientation>();
    protected EntityEffects EffectsComp         => Entity.Get<EntityEffects>();
    protected EntityVisuals VisualsComp         => Entity.Get<EntityVisuals>();
    protected EntityVFX VFXComp                 => Entity.Get<EntityVFX>();
    protected EntityPhysics PhysicsComp         => Entity.Get<EntityPhysics>();
    protected EntityAnimator AnimatorComp       => Entity.Get<EntityAnimator>();
    protected EntityAudio AudioComp             => Entity.Get<EntityAudio>();
    protected ComboTracker ComboComp            => Entity.Get<ComboTracker>();
    protected ShakeController ShakeComp         => Entity.Get<ShakeController>();
    protected InputReader InputComp             => Entity.Get<InputReader>();

    // Opponent
    protected Entity Opponent                   => Entity.Opponent;
}
