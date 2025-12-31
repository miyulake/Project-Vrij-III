using Game.Entities;
using UnityEngine;

public class TauntHandler : EntityComponent, ITickable, IResettable
{
    public bool HasCompletedTaunt { get; private set; }
    [SerializeField] private GameObject m_PowerUpParticle;
    [SerializeField] private AudioClip m_Active;
    [SerializeField] private AudioClip m_Inactive;
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
            AudioComp.Play(m_Active);
        }
        else AudioComp.Play(m_Inactive);
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

    public void Reset()
    {
        HasCompletedTaunt = false;
        m_PowerUpParticle.SetActive(false);
        m_PowerTime = 0f;
    }
}
