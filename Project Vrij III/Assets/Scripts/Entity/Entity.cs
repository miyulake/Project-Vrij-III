using UnityEngine;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(AttackHandler))]
[RequireComponent(typeof(EntityHealth))]
[RequireComponent(typeof(EntityPhysics))]
[RequireComponent(typeof(EntityVFX))]
[RequireComponent(typeof(EntityVisuals))]
[RequireComponent(typeof(EntityAudio))]
[RequireComponent(typeof(EntityOrientation))]
[RequireComponent(typeof(EntityAnimator))]
[RequireComponent(typeof(ShakeController))]
[RequireComponent(typeof(InputReader))]
public class Entity : MonoBehaviour
{
    public StateMachine      StateMachine { get; private set; }
    public AttackHandler     Attack { get; private set; }
    public EntityHealth      Health { get; private set; }
    public EntityPhysics     Physics { get; private set; }
    public EntityResolver    Resolver { get; private set; }
    public EntityVFX         VFX { get; private set; }
    public EntityVisuals     Visuals { get; private set; }
    public EntityAudio       Audio { get; private set; }
    public EntityOrientation Orientation { get; private set; }
    public EntityAnimator    Animator { get; private set; }
    public ShakeController   Shake { get; private set; }
    public ComboTracker      Combo { get; private set; }
    public InputReader       Input { get; private set; }
    public Entity            Opponent { get; private set; }

    private void Awake()
    {
        StateMachine = GetComponent<StateMachine>();
        Attack       = GetComponent<AttackHandler>();
        Health       = GetComponent<EntityHealth>();
        Physics      = GetComponent<EntityPhysics>();
        Resolver     = new EntityResolver(this);
        VFX          = GetComponent<EntityVFX>();
        Visuals      = GetComponent<EntityVisuals>();
        Audio        = GetComponent<EntityAudio>();
        Orientation  = GetComponent<EntityOrientation>();
        Animator     = GetComponent<EntityAnimator>();
        Shake        = GetComponent<ShakeController>();
        Combo        = new ComboTracker();
        Input        = GetComponent<InputReader>();
    }

    private void FixedUpdate()
    {
        StateMachine.Tick();
        Attack.Tick();
        Orientation.Tick();
        Animator.Tick();
    }

    public void ResetEntity()
    {
        Attack.Reset();
        Health.Reset();
        Visuals.Reset();
        Orientation.Reset();
        Animator.Play("Idle");
        Combo.Reset();
        StateMachine.OverrideChangeState<IdleState>();
    }

    public void SetOpponent(Entity opponent) => Opponent = opponent;
}
