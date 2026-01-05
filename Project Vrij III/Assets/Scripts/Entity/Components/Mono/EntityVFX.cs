using UnityEngine;

public class EntityVFX : EntityComponent
{
    [SerializeField] private Transform m_ParticleSpawn;
    [SerializeField] private float m_ParticleZ = -3f;
    private PaintSettings m_PaintSettings;

    public Color PaintColor => m_PaintSettings.color;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        m_PaintSettings = Entity.Character.Paint;
    }

    public void SpawnParticles(ContactData data)
    {
        if (data.particleEffect == null) return;

        var position = m_ParticleSpawn.position;
        Instantiate(data.particleEffect, new Vector3(position.x, position.y, m_ParticleZ), Quaternion.identity);
    }

    public void SpawnPaint(MoveData move, int facing)
    {
        if (move.paintData.paintPrefab == null) return;

        var position = new Vector3(
            transform.position.x,
            transform.position.y,
            PaintManager.Instance.GetPaintZ());
        var offset = new Vector3(
            move.paintData.offsetPosition.x * -facing,
            move.paintData.offsetPosition.y,
            move.paintData.offsetPosition.z
        );
        var paint = Instantiate(
            move.paintData.paintPrefab,
            position + offset,
            Quaternion.identity
        );

        var scale = move.paintData.paintScale;
        paint.transform.localScale = new Vector3(scale.x * -facing, scale.y, 1);

        var renderer = paint.GetComponent<Renderer>();
        var block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        block.SetColor("_BaseColor", Opponent.Get<EntityVFX>().PaintColor);
        renderer.SetPropertyBlock(block);
    }
}
