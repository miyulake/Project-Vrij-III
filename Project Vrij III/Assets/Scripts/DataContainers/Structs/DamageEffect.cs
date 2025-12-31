using UnityEngine;

[System.Serializable]
public struct DamageEffect
{
    [Range(0, 1)]
    public float additiveMultiplier;
    [Range(0, 10)]
    public int flatIncrease;
}