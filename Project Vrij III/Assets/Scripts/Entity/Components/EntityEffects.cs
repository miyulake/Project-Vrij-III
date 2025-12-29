using Game.Entities;
using UnityEngine;

[System.Serializable]
public struct DamageEffect
{
    [Range(0, 1)] public float additiveMultiplier;
    [Range(0, 10)] public int flatIncrease;
}

public class EntityEffects : EntityComponent
{
    [SerializeField] private DamageEffect m_TauntEffect;

    public float GetMultiplier()
    {
        // Taunt
        var completedTaunt = TauntComp.HasCompletedTaunt;
        var tauntMultiplier = completedTaunt ? m_TauntEffect.additiveMultiplier : 0;

        // Super


        // Total
        var totalMultiplier = 1 + tauntMultiplier; // Add any other mutlipliers here
        return totalMultiplier;
    }

    public int GetFlatIncrease()
    {
        // Taunt
        var completedTaunt = TauntComp.HasCompletedTaunt;
        var tauntIncrease = completedTaunt ? m_TauntEffect.flatIncrease : 0;

        // Super


        // Total
        var totalIncrease = tauntIncrease; // Add any other increases here
        return totalIncrease;
    }
}
