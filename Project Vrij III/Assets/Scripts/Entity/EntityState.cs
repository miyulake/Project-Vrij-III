public abstract class EntityState
{
    protected Entity Entity;
    protected StateMachine StateMachine;

    protected EntityState(Entity entity, StateMachine stateMachine) 
    {
        Entity = entity;
        StateMachine = stateMachine;
    }

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Tick() { }
}
