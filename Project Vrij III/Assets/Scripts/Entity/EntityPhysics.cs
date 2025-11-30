using UnityEngine;

public class EntityPhysics : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_RigidBodyTwoD;

    public void ApplyKnockback(Vector2 knockback)
    {
        m_RigidBodyTwoD.linearVelocity = Vector2.zero;
        m_RigidBodyTwoD.AddForce(knockback, ForceMode2D.Impulse);
    }
}