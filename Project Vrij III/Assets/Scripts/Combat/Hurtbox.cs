using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public int ownerId;
    [SerializeField] private Entity entity;

    private void Start() => entity = GetComponentInParent<Entity>();
    public void ApplyHit(AttackInfo attackInfo) => entity.ReceiveHit(attackInfo); // Send attack info to Entity script
    //public void ApplyMove(MoveData move) =>
}