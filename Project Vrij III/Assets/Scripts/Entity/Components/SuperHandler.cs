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
        m_AllSupers = UnityEngine.Resources.LoadAll<SuperData>("MoveData");
    }

    public void Tick()
    {
        if (Input.Super && CanExecuteSuper()) ExecuteSuper(GetSuperData());
    }

    private SuperData GetSuperData()
    {
        for (int i = 0; i < m_AllSupers.Length; i++)
        {
            if (m_AllSupers[i].superType == m_CurrentSuper) return m_AllSupers[i];
        }
        return null;
    }

    public bool CanExecuteSuper() => GetSuperData().CanExecute(Resources);

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
