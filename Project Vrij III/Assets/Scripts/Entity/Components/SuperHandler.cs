using Game.Entities;
using UnityEngine;

public class SuperHandler : EntityComponent, ITickable
{
    [SerializeField] private SuperType m_CurrentSuper;
    [SerializeField] private GameObject m_Aura;
    private SuperData[] m_AllSupers;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        m_AllSupers = Resources.LoadAll<SuperData>("MoveData");
    }

    public void Tick()
    {
        if (StateMachine.CurrentState is SuperState || RoundManager.Instance.CurrentState != RoundState.GAMEPLAY) return;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1)) ResourcesComp.AddMeter(GameManager.Instance.GetMaxMeter());
#endif

        Debug.Log($"Meter before super: {ResourcesComp.Meter.Current}");
        var currentSuper = GetSuperData();
        if (InputComp.Super && currentSuper.CanExecute(ResourcesComp)) ExecuteSuper(GetSuperData());
    }

    private void ExecuteSuper(SuperData data)
    {
        switch (data.superType)
        {
            case SuperType.CHAIN:
                //ActivateChain();
                break;

            case SuperType.TIME:
                //ActivateTime();
                break;

            case SuperType.TAUNT:
                ActivateTaunt();
                break;
        }
        StateMachine.ChangeState<SuperState>(false, data.freezeFrames, data.activationFrames);
        ResourcesComp.Meter.Modify(-data.meterCost);
        Debug.Log($"Meter after super: {ResourcesComp.Meter.Current}");
    }

    public void ExitSuper(SuperData data)
    {
        switch (data.superType)
        {
            case SuperType.CHAIN:
                //DeactivateChain();
                break;

            case SuperType.TIME:
                //DeactivateTime();
                break;

            case SuperType.TAUNT:
                DeactivateTaunt();
                break;
        }
    }

    private void ActivateTaunt()
    {
        m_Aura.SetActive(true);
    }

    private void DeactivateTaunt()
    {
        m_Aura.SetActive(false);
    }

    public SuperData GetSuperData()
    {
        for (int i = 0; i < m_AllSupers.Length; i++)
        {
            if (m_AllSupers[i].superType == m_CurrentSuper) return m_AllSupers[i];
        }
        return null;
    }
}
