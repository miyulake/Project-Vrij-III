using Game.Entities;
using UnityEngine;

public class EntityResources : EntityComponent, IResettable
{
    public int CurrentHealth { get; private set; }
    public int CurrentMeter { get; private set; }

    private int m_MaxHealth;
    private int m_MaxMeter;

    private bool UsingResources => 
        RoundManager.Instance.CurrentState == RoundState.GAMEPLAY &&
        GameManager.Instance.CurrentMode != GameMode.PAINT;

    public void Start()
    {
        m_MaxHealth = GameManager.Instance.GetMaxHealth();
        m_MaxMeter = 100; // TEST

        CurrentHealth = m_MaxHealth;
        CurrentMeter = 0;
    }

    public void ApplyDamage(int damage)
    {
        if (!UsingResources) return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            StateMachine.ChangeState<DeadState>();
            RoundManager.Instance.SetState(RoundState.KNOCKOUT);
        }
    }

    public void ConsumeMeter(int meter)
    {
        if (!UsingResources) return;

        CurrentMeter -= meter;
        CurrentHealth = Mathf.Clamp(meter, 0, m_MaxMeter);
    }

    public void AddMeter(int meter)
    {
        if (!UsingResources) return;

        CurrentMeter += meter;
        CurrentHealth = Mathf.Clamp(meter, 0, m_MaxMeter);
    }

    public void Reset() => CurrentHealth = m_MaxHealth;
}
