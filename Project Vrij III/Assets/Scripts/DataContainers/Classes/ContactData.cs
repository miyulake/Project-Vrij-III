using UnityEngine;

[System.Serializable]
public class ContactData
{
    public int stun;
    public int damage;
    public Vector2 knockback;
    public float shakeMagnitude;
    [Space]
    public GameObject particleEffect;
    public AudioClip sound;
}