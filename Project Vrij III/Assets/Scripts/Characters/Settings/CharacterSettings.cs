using UnityEngine;
using System;

// Centralized settings class
[Serializable]
public class CharacterSettings
{
    public MovementSettings movement;
    public EffectSettings effects;
    public VisualsSettings visuals;
    public PaintSettings paint;
}

[Serializable]
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

[Serializable]
public class EffectSettings
{
    [Range(5, 10)]
    public float tauntEffectDuration = 7;
    public DamageEffect taunt;
}

[Serializable]
public class VisualsSettings
{
    public Material baseMaterial;
    public Material deadMaterial;
}

[Serializable]
public class PaintSettings
{
    [ColorUsage(true, true)]
    public Color color = Color.red;
}

// Add more customizable settings below...