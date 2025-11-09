using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public MoveData MoveData { private get; set; }
    [SerializeField] private int m_OwnerId;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox hurtbox) && m_OwnerId == hurtbox.ownerId) return;
        //hurtbox.ApplyMove(MoveData);
    }
}
