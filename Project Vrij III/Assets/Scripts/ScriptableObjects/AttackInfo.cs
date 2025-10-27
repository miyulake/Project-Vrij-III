using UnityEngine;

[CreateAssetMenu(fileName = "AttackInfo", menuName = "CombatSO/Attack")]
public class AttackInfo : ScriptableObject
{
    [Header("Paint Settings")]
    public GameObject paintPrefab;
    public Vector3 offsetPosition;
    public Vector3 paintScale = new Vector3(1,1,1);
    public Quaternion paintRotation;

    [Header("Attack Settings")]
    public Vector2 knockback = new(0,0);
    public ForceMode2D attackForceMode = ForceMode2D.Impulse;
    public float hitstunDuration = 0;
    public float shakeMagnitude = 0;

    [Header("Player Settings")]
    public Vector2 momentum = new(0,0);
    public ForceMode2D momentumForceMode = ForceMode2D.Impulse;
}