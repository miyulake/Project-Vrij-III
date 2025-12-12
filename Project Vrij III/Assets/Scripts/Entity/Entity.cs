using UnityEngine;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(AttackHandler))]
[RequireComponent(typeof(EntityPhysics))]
[RequireComponent(typeof(EntityVFX))]
[RequireComponent(typeof(EntityVisuals))]
[RequireComponent(typeof(EntityAudio))]
[RequireComponent(typeof(EntityOrientation))]
[RequireComponent(typeof(EntityAnimator))]
[RequireComponent(typeof(ThrowLogic))]
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
    public ThrowLogic        Throw { get; private set; }
    public ShakeController   Shake { get; private set; }
    public ComboTracker      Combo { get; private set; }
    public InputReader       Input { get; private set; }
    public Entity            Opponent { get; private set; }

    private void Awake()
    {
        CacheComponents();

        Health   = new EntityHealth(this);
        Resolver = new EntityResolver(this);
        Combo    = new ComboTracker();
    }

    private void Start()
    {
        Health.Start();
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
        Animator.Play("Start");
        Combo.Reset();
        StateMachine.ChangeState<IdleState>(true);
    }

    public void PauseEntity()
    {
        Attack.SetPauseState(true);
        Animator.Pause();
    }

    public void ResumeEntity()
    {
        Attack.SetPauseState(false);
        Animator.Resume();
    }

    public void SetOpponent(Entity opponent) => Opponent = opponent;

    private void CacheComponents()
    {
        StateMachine = GetComponent<StateMachine>();
        Attack       = GetComponent<AttackHandler>();
        Physics      = GetComponent<EntityPhysics>();
        VFX          = GetComponent<EntityVFX>();
        Visuals      = GetComponent<EntityVisuals>();
        Audio        = GetComponent<EntityAudio>();
        Orientation  = GetComponent<EntityOrientation>();
        Animator     = GetComponent<EntityAnimator>();
        Throw        = GetComponent<ThrowLogic>();
        Shake        = GetComponent<ShakeController>();
        Input        = GetComponent<InputReader>();
    }
}
