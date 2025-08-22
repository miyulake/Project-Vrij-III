using UnityEngine;

[System.Serializable]
public class AttackInfo
{
    public Vector2 knockback = new(0,0);
    public ForceMode2D forceMode = ForceMode2D.Impulse;
    public float hitstunDuration = 0;
    public float shakeMagnitude = 0;
}