using UnityEngine;

public class EntityPhysics
{
    public void ApplyKnockback(Rigidbody2D rigidBody, Vector2 knockback)
    {
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.AddForce(knockback, ForceMode2D.Impulse);
    }
}