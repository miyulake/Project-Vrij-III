using UnityEngine;

public class EntityVFX : EntityComponent
{
    [SerializeField] private Transform particleSpawn;
    [SerializeField] private Transform paintLayer;
    [SerializeField] private float particleZ = -3f;
    [SerializeField] private Color opponentColor = Color.blue;

    public void SpawnParticles(ContactData data)
    {
        if (data.particleEffect == null) return;

        var position = particleSpawn.position;
        Instantiate(data.particleEffect, new Vector3(position.x, position.y, particleZ), Quaternion.identity);
    }

    public void SpawnPaint(MoveData move, int facing)
    {
        if (move.paintData.paintPrefab == null) return;

        var position = transform.position;
        var offset = new Vector3(
            move.paintData.offsetPosition.x * -facing,
            move.paintData.offsetPosition.y,
            move.paintData.offsetPosition.z
        );

        var paint = Instantiate(
            move.paintData.paintPrefab,
            position + offset,
            Quaternion.identity,
            paintLayer
        );

        var scale = move.paintData.paintScale;
        paint.transform.localScale = new Vector3(scale.x * -facing, scale.y, scale.z);

        var renderer = paint.GetComponent<Renderer>();
        var block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        block.SetColor("_BaseColor", opponentColor);
        renderer.SetPropertyBlock(block);
    }
}
