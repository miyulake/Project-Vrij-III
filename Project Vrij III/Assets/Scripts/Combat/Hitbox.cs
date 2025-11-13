using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public MoveData MoveData { private get; set; }
    [SerializeField] private Entity m_Entity;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox hurtbox) || m_Entity.iD == hurtbox.entity.iD) return;
        hurtbox.ApplyMove(MoveData);
    }
}