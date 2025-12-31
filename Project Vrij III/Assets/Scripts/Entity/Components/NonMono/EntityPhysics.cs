using Game.Entities;
using UnityEngine;

public class EntityPhysics : IEntityComponent
{
    public void Initialize(Entity entity) { }

    public void ApplyKnockback(Rigidbody2D rigidBody, Vector2 knockback)
    {
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.AddForce(knockback, ForceMode2D.Impulse);
    }
}