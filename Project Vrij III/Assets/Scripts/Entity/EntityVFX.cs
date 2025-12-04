using UnityEngine;

public class EntityVFX : EntityComponent
{
    [SerializeField] private Transform m_ParticleSpawn;
    [SerializeField] private Transform m_PaintLayer;
    [SerializeField] private float m_ParticleZ = -3f;
    [SerializeField] private Color m_OpponentColor = Color.blue;

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
            m_PaintLayer.position.z);
        var offset = new Vector3(
            move.paintData.offsetPosition.x * -facing,
            move.paintData.offsetPosition.y,
            move.paintData.offsetPosition.z
        );
        var paint = Instantiate(
            move.paintData.paintPrefab,
            position + offset,
            Quaternion.identity,
            m_PaintLayer
        );

        var scale = move.paintData.paintScale;
        paint.transform.localScale = new Vector3(scale.x * -facing, scale.y, 1);

        var renderer = paint.GetComponent<Renderer>();
        var block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        block.SetColor("_BaseColor", m_OpponentColor);
        renderer.SetPropertyBlock(block);
    }
}
