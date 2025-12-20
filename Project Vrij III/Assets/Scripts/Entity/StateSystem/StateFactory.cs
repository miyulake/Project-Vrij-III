using System;
using System.Linq;

public class StateFactory
{
    private readonly Entity m_Entity;

    public StateFactory(Entity entity) => m_Entity = entity;

    public T Create<T>(params object[] args) where T : EntityState
    {
        // Don't ask me how this works
        var finalArgs = new object[] { m_Entity }.Concat(args).ToArray();
        return (T)Activator.CreateInstance(typeof(T), finalArgs);
    }
}