public abstract class EntityState : EntityContext
{
    protected EntityState(Entity entity) => SetEntity(entity);

    public virtual void OnEnter() { }
    public virtual void Tick() { }
    public virtual void OnExit() { }
}
