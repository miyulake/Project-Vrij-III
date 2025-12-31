using Game.Entities.Resources;
using Game.Entities;

public class EntityResources : EntityContext, IEntityComponent, IResettable
{
    public IResource Health { get; private set; }
    public IResource Meter { get; private set; }

    private bool UsingResources =>
        RoundManager.Instance.CurrentState == RoundState.GAMEPLAY &&
        GameManager.Instance.CurrentMode != GameMode.PAINT;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);

        var maxHealth = GameManager.Instance.GetMaxHealth();
        var maxMeter = GameManager.Instance.GetMaxMeter();

        Health = new Resource(maxHealth, maxHealth);
        Meter = new Resource(maxMeter, 0);

        Health.Emptied += OnHealthEmptied;
    }

    private void OnHealthEmptied()
    {
        StateMachine.ChangeState<DeadState>();
        RoundManager.Instance.SetState(RoundState.KNOCKOUT);
    }

    public void ApplyDamage(int damage)
    {
        if (!UsingResources) return;
        Health.Modify(-damage);
    }

    public void ConsumeMeter(int meter)
    {
        if (!UsingResources) return;
        Meter.Modify(-meter);
    }

    public void AddMeter(int meter)
    {
        if (!UsingResources) return;
        Meter.Modify(meter);
    }

    public void Reset() => Health.Set(Health.Max);
}
