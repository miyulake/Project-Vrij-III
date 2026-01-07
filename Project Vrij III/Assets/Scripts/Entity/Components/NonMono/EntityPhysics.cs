using Game.Entities;
using UnityEngine;

public class EntityPhysics : EntityContext, IEntityComponent
{
    public Rigidbody2D RigidBodyTwoD { get; private set; }

    public void Initialize(Entity entity) 
    {
        SetEntity(entity);
        RigidBodyTwoD = Entity.GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Vector2 knockback)
    {
        RigidBodyTwoD.linearVelocity = Vector2.zero;
        RigidBodyTwoD.AddForce(knockback, ForceMode2D.Impulse);
    }
}