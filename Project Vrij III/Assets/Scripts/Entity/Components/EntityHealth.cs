public class EntityHealth
{
    public int CurrentHealth { get; private set; }
    private int m_MaxHealth;
    private readonly Entity m_Entity;

    public EntityHealth(Entity entity) => m_Entity = entity;

    public void Start()
    {
        m_MaxHealth = GameManager.Instance.maxHealth;
        CurrentHealth = m_MaxHealth;
    }

    public void ApplyDamage(int damage)
    {
        if (RoundManager.Instance.CurrentState != RoundState.GAMEPLAY) return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            m_Entity.StateMachine.ChangeState<DeadState>();
            RoundManager.Instance.SetState(RoundState.KNOCKOUT);
        }
    }

    public void Reset() => CurrentHealth = m_MaxHealth;
}
