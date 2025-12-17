using UnityEngine;

public class TauntHandler : EntityComponent
{
    public bool HasCompletedTaunt { get; private set; }
    [SerializeField] private GameObject m_PowerUpParticle;
    [SerializeField] private AudioClip m_Active;
    [SerializeField] private AudioClip m_Inactive;
    [SerializeField] private float m_Multiplier = 1.15f;
    [SerializeField] private int m_FlatIncrease = 1;
    [SerializeField] private float m_PowerDuration = 10f;
    private float m_PowerTime = 0f;

    public void Tick() => HandlePowerTimer();

    public void SetTauntPower(bool state)
    {
        var canPowerUp =
            GameManager.Instance.CurrentMode != GameMode.PAINT &&
            RoundManager.Instance.CurrentState == RoundState.GAMEPLAY;

        if (!canPowerUp) return;

        HasCompletedTaunt = state;

        m_PowerUpParticle.SetActive(HasCompletedTaunt);

        if (HasCompletedTaunt)
        {
            m_PowerTime = 0f;
            Entity.Audio.Play(m_Active);
        }
        else Entity.Audio.Play(m_Inactive);
    }

    private void HandlePowerTimer()
    {
        if (HasCompletedTaunt)
        {
            m_PowerTime += Time.deltaTime;
            if (m_PowerTime >= m_PowerDuration)
            {
                SetTauntPower(false);
                HasCompletedTaunt = false;
                m_PowerTime = 0f;
            }
        }
    }

    // Bad - should add to a global mutliplier
    public float GetMultiplier() =>
        HasCompletedTaunt ? m_Multiplier : 1f;

    // Bad - should add to a global flat increase
    public int GetFlatIncrease() =>
        HasCompletedTaunt ? m_FlatIncrease : 0;

    public void Reset()
    {
        HasCompletedTaunt = false;
        m_PowerUpParticle.SetActive(false);
        m_PowerTime = 0f;
    }
}
