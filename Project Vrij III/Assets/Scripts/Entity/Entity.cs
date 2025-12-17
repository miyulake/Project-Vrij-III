using UnityEngine;

[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(AttackHandler))]
[RequireComponent(typeof(SuperHandler))]
[RequireComponent(typeof(ThrowHandler))]
[RequireComponent(typeof(TauntHandler))]
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
    public TwoDMovement      Movement { get; private set; }
    public AttackHandler     Attack { get; private set; }
    public SuperHandler      Super { get; private set; }
    public ThrowHandler      Throw { get; private set; }
    public TauntHandler      Taunt { get; private set; }
    public EntityResources   Resources { get; private set; }
    public EntityResolver    Resolver { get; private set; }
    public EntityPhysics     Physics { get; private set; }
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
        CacheComponents();

        Resources = new EntityResources(this);
        Resolver  = new EntityResolver(this);
        Physics   = new EntityPhysics();
        Combo     = new ComboTracker();
    }

    private void Start() => Resources.Start();

    private void FixedUpdate() => Tick();

    private void Tick()
    {
        StateMachine.Tick();
        Attack.Tick();
        Super.Tick();
        Orientation.Tick();
        Animator.Tick();
        Taunt.Tick();
    }

    public void Reset()
    {
        Attack.Reset();
        Resources.Reset();
        Taunt.Reset();
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

    private void CacheComponents()
    {
        StateMachine = GetComponent<StateMachine>();
        Attack       = GetComponent<AttackHandler>();
        Super        = GetComponent<SuperHandler>();
        Throw        = GetComponent<ThrowHandler>();
        Taunt        = GetComponent<TauntHandler>();
        Movement     = GetComponent<TwoDMovement>();
        VFX          = GetComponent<EntityVFX>();
        Visuals      = GetComponent<EntityVisuals>();
        Audio        = GetComponent<EntityAudio>();
        Orientation  = GetComponent<EntityOrientation>();
        Animator     = GetComponent<EntityAnimator>();
        Shake        = GetComponent<ShakeController>();
        Input        = GetComponent<InputReader>();
    }

    public void SetOpponent(Entity opponent) => Opponent = opponent;
}
