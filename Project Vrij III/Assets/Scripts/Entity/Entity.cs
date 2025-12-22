using Game.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityPhysics Physics { get; private set; }
    public ComboTracker Combo { get; private set; }
    public Entity Opponent { get; private set; }

    private List<IEntityComponent> m_Components = new();
    private Dictionary<Type, IEntityComponent> m_ComponentMap;

    private void Awake()
    {
        CacheComponents();
        for (int i = 0; i < m_Components.Count; i++) m_Components[i].Initialize(this);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < m_Components.Count; i++)
        {
            if (m_Components[i] is ITickable tickable) tickable.Tick();
        }
    }

    public void Pause()
    {
        for (int i = 0; i < m_Components.Count; i++)
        {
            if (m_Components[i] is IPausable pausable) pausable.Pause();
        }
    }

    public void Resume()
    {
        for (int i = 0; i < m_Components.Count; i++)
        {
            if (m_Components[i] is IPausable pausable) pausable.Resume();
        }
    }

    public void Reset()
    {
        for (int i = 0; i < m_Components.Count; i++)
        {
            if (m_Components[i] is IResettable resettable) resettable.Reset();
        }
        Get<EntityAnimator>().Play("Start");
        Get<StateMachine>().ChangeState<IdleState>(true);
    }

    private void CacheComponents()
    {
        Physics = new EntityPhysics();
        Combo = new ComboTracker();

        m_Components = new List<IEntityComponent>(GetComponents<IEntityComponent>());
        m_ComponentMap = new Dictionary<Type, IEntityComponent>(m_Components.Count)
        {
            [typeof(EntityPhysics)] = Physics,
            [typeof(ComboTracker)] = Combo
        };

        for (int i = 0; i < m_Components.Count; i++)
        {
            Debug.Log("Caching component: " + m_Components[i].GetType().Name);
            m_ComponentMap[m_Components[i].GetType()] = m_Components[i];
        }
    }

    public T Get<T>() where T : class, IEntityComponent =>
        m_ComponentMap.TryGetValue(typeof(T), out var c) ? c as T : null;

    public void SetOpponent(Entity opponent) => Opponent = opponent;
}
