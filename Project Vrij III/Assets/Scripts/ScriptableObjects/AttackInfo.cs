using UnityEngine;

[CreateAssetMenu(fileName = "AttackInfo", menuName = "CombatSO/Attack")]
public class AttackInfo : ScriptableObject
{
    [Header("Paint Settings")]
    public GameObject paintSplatter;
    public Vector3 paintScale;
    public Vector3 paintRotation;

    [Header("Attack Settings")]
    public Vector2 knockback = new(0,0);
    public ForceMode2D forceMode = ForceMode2D.Impulse;
    public float hitstunDuration = 0;
    public float shakeMagnitude = 0;
}