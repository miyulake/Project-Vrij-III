public class EntityHealth : EntityComponent
{
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;
    private int m_MaxHealth;

    private void Start()
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
            Entity.StateMachine.ChangeState<DeadState>();
            RoundManager.Instance.SetState(RoundState.KNOCKOUT);
        }
    }

    public void Reset() => CurrentHealth = m_MaxHealth;
}
