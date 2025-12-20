using Game.Entities;

public class ComboTracker : IEntityComponent, IResettable
{
    public int Hits { get; private set; }
    public int Damage { get; private set; }

    public void Initialize(Entity entity) { }

    public void AddHit(int damage)
    {
        ++Hits;
        Damage += damage;
    }

    public void Reset()
    {
        Hits = 0;
        Damage = 0;
    }
}
