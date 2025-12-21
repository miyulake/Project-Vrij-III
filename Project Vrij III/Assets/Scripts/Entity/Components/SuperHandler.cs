using UnityEngine;

public class SuperHandler : EntityComponent
{
    [SerializeField] private SuperType m_CurrentSuper;
    [SerializeField] private GameObject m_Aura;
    private SuperData[] m_AllSupers;

    protected override void Awake()
    {
        base.Awake();
        m_AllSupers = Resources.LoadAll<SuperData>("MoveData");
    }

    public void Tick()
    {
        if (Entity.Input.Super && CanExecuteSuper()) ExecuteSuper(GetSuperData());
    }

    private SuperData GetSuperData()
    {
        for (int i = 0; i < m_AllSupers.Length; i++)
        {
            if (m_AllSupers[i].superType == m_CurrentSuper) return m_AllSupers[i];
        }
        return null;
    }

    public bool CanExecuteSuper() => GetSuperData().CanExecute(Entity);

    private void ExecuteSuper(SuperData data)
    {
        switch (data.superType)
        {
            case SuperType.CHAIN:
                //ActivateChain(data);
                break;

            case SuperType.TIME:
                //ActivateTime(data);
                break;

            case SuperType.TAUNT:
                //ActivateTaunt(data);
                break;
        }
        Entity.StateMachine.ChangeState<SuperState>();
    }

}
