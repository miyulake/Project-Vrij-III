using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Hurtbox : MonoBehaviour
{
    public Entity entity;
    public void ApplyMove(MoveData move) => entity.ReceiveMove(move);
}