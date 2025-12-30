using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private Entity m_Entity;
    [SerializeField] private MoveData m_MoveData;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Entity hitEntity) || hitEntity == m_Entity) return;

        if (m_MoveData.moveType == MoveType.GRAB && m_Entity != null) m_Entity.Get<ThrowHandler>().ConnectGrab();

        hitEntity.Get<EntityResolver>().ResolveHit(m_MoveData);
    }

    public MoveData SetMoveData(MoveData data) => m_MoveData = data;
}