using UnityEngine;

// Centralized settings class
[System.Serializable]
public class CharacterSettings
{
    public MovementSettings movement;
    public EffectSettings effects;
    public PaintSettings paint;
}

[System.Serializable]
public class MovementSettings
{
    [Range(1, 20)]
    public float baseSpeed = 10f;
    [Range(0, 20)]
    public float blockSpeed = 3f;
    [Range(10, 500)]
    public float acceleration = 250f;
    [Range(0, 100)]
    public float deceleration = 50f;
}

[System.Serializable]
public class EffectSettings
{
    public DamageEffect taunt;
}

[System.Serializable]
public class PaintSettings
{
    [ColorUsage(true, true)]
    public Color color = Color.red;
}

// Add more customizable settings below...