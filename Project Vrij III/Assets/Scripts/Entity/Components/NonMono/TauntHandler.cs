using Game.Entities;
using UnityEngine;

public class TauntHandler : EntityContext, IEntityComponent, ITickable, IResettable
{
    public bool HasCompletedTaunt { get; private set; }
    private EffectSettings m_Settings;
    private float m_PowerTime = 0f;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_Settings = Entity.Character.Effects;
    }

    public void Tick() => HandlePowerTimer();

    public void SetTauntPower(bool state)
    {
        var canPowerUp =
            GameManager.Instance.CurrentMode != GameMode.PAINT &&
            RoundManager.Instance.CurrentState == RoundState.GAMEPLAY;

        if (!canPowerUp) return;

        HasCompletedTaunt = state;

        ViewComp.TauntEffect.SetActive(HasCompletedTaunt);

        if (HasCompletedTaunt)
        {
            m_PowerTime = 0f;
            AudioComp.GetPlay(SoundType.TauntActive);
        }
        else AudioComp.GetPlay(SoundType.TauntInactive);
    }

    private void HandlePowerTimer()
    {
        if (HasCompletedTaunt)
        {
            m_PowerTime += Time.deltaTime;
            if (m_PowerTime >= m_Settings.tauntEffectDuration)
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
        ViewComp.TauntEffect.SetActive(false);
        m_PowerTime = 0f;
    }
}
