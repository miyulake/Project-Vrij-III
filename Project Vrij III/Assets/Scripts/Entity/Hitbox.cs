using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public MoveData MoveData { private get; set; }
    [SerializeField] private Entity m_Entity;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Entity entity) || entity == m_Entity) return;
        entity.Resolver.ResolveHit(MoveData);
    }
}