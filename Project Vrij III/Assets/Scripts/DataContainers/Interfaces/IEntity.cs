namespace Game.Entities
{
    public interface IEntityComponent { void Initialize(Entity entity); }
    public interface ITickable { void Tick(); }
    public interface IResettable { void Reset(); }
}