using Game.Entities;

public class EntityEffects : EntityContext, IEntityComponent
{
    private EffectSettings m_Settings;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_Settings = Entity.GetCharacter().GetEffects();
    }

    public float GetMultiplier()
    {
        // Taunt
        var completedTaunt = TauntComp.HasCompletedTaunt;
        var tauntMultiplier = completedTaunt ? m_Settings.taunt.additiveMultiplier : 0;

        // Super


        // Total
        var totalMultiplier = 1 + tauntMultiplier; // Add any other mutlipliers here
        return totalMultiplier;
    }

    public int GetFlatIncrease()
    {
        // Taunt
        var completedTaunt = TauntComp.HasCompletedTaunt;
        var tauntIncrease = completedTaunt ? m_Settings.taunt.flatIncrease : 0;

        // Super


        // Total
        var totalIncrease = tauntIncrease; // Add any other increases here
        return totalIncrease;
    }
}
