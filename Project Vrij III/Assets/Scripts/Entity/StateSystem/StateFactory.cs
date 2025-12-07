using System;
using System.Linq;

public class StateFactory
{
    private readonly Entity m_Entity;
    private readonly StateMachine m_StateMachine;

    public StateFactory(Entity entity, StateMachine machine)
    {
        m_Entity = entity;
        m_StateMachine = machine;
    }

    public T Create<T>(params object[] args) where T : EntityState
    {
        // Don't ask me how this works
        var finalArgs = new object[] { m_Entity, m_StateMachine }.Concat(args).ToArray();
        return (T)Activator.CreateInstance(typeof(T), finalArgs);
    }
}