using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public MoveData MoveData { private get; set; }
    private Entity m_Entity;

    private void Start() => m_Entity = GetComponentInParent<Entity>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox hurtbox)) return;
        if (m_Entity.iD != hurtbox.entity.iD)
        {
            hurtbox.ApplyMove(MoveData);
            Debug.Log($"Hit: {col.gameObject.name}");
        }
    }
}
