using UnityEngine;

public class Hitbox : EntityComponent
{
    public MoveData MoveData { private get; set; }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Entity hitEntity) || hitEntity == Entity) return;

        if (MoveData.moveType == MoveType.GRAB) Throw.ConnectGrab();

        Resolver.ResolveHit(MoveData);
    }
}