using UnityEngine;

public class SuperHandler : EntityComponent
{
    [SerializeField] private SuperType m_CurrentSuper;
    [SerializeField] private SuperData[] m_SuperData;

    public void Tick()
    {
        if (Entity.Input.Super)
            ExecuteSuper(GetSuperData(m_CurrentSuper));
    }

    private SuperData GetSuperData(SuperType type)
    {
        for (int i = 0; i < m_SuperData.Length; i++)
        {
            if (m_SuperData[i].superType == type)
                return m_SuperData[i];
        }
        return null;
    }

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
    }

}
