using UnityEngine;

[CreateAssetMenu(fileName = "AttackInfo", menuName = "CombatSO/Attack")]
public class AttackInfo : ScriptableObject
{
    [Header("Paint Settings")]
    public GameObject paintPrefab;
    public Vector3 offsetPosition;
    public Vector3 paintScale = new(1,1,1);
    public Quaternion paintRotation = new(0,0,0,0);

    [Header("Attack Settings")]
    public Vector2 knockback = new(0,0);
    public ForceMode2D attackForceMode = ForceMode2D.Impulse;
    public float hitStunDuration = 0;
    public float blockStunDuration = 0;
    public float shakeMagnitude = 0;

    [Header("Feedback Settings")]
    public GameObject hitParticle;
    public AudioClip hitSound;

    /*
    [Header("Player Settings")]
    public Vector2 momentum = new(0,0);
    public ForceMode2D momentumForceMode = ForceMode2D.Impulse;
    */
}