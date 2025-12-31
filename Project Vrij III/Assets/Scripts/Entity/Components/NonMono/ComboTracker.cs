using Game.Entities;
using System;

public class ComboTracker : IEntityComponent, IPausable, IResettable
{
    public bool IsPaused { get; private set; }
    public int Hits { get; private set; }
    public int Damage { get; private set; }

    public event Action<int, int> OnComboUpdated;

    public void Initialize(Entity entity) { }

    public void AddHit(int damage)
    {
        ++Hits;
        Damage += damage;
        OnComboUpdated?.Invoke(Hits, Damage);
    }

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;

    public void Reset()
    {
        Hits = 0;
        Damage = 0;
        OnComboUpdated?.Invoke(Hits, Damage);
    }
}
