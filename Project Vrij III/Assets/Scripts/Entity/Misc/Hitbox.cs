using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private Entity m_Entity;
    public MoveData MoveData { private get; set; }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Entity hitEntity) || hitEntity == m_Entity) return;

        if (MoveData.moveType == MoveType.GRAB) m_Entity.Get<ThrowHandler>().ConnectGrab();

        hitEntity.Get<EntityResolver>().ResolveHit(MoveData);
    }
}