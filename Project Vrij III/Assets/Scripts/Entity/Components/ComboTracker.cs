public class ComboTracker
{
    public int Hits { get; private set; }
    public int Damage { get; private set; }

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
