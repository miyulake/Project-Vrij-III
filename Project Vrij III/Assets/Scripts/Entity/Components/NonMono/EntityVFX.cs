using UnityEngine;
using Game.Entities;

public class EntityVFX : EntityContext, IEntityComponent
{
    private CombatSettings m_CombatSettings;
    private PaintSettings m_PaintSettings;

    public Color PaintColor => m_PaintSettings.color;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_CombatSettings = Entity.Character.combat;
        m_PaintSettings = Entity.Character.Paint;
    }

    public void SpawnParticles(ContactData data)
    {
        if (data.particleEffect == null) return;

        var twoDeePosition = ViewComp.ParticleSpawn.position;
        var targetPosition = new Vector3(twoDeePosition.x, twoDeePosition.y, m_CombatSettings.particleZPosition);
        Object.Instantiate(data.particleEffect, targetPosition, Quaternion.identity);
    }

    public void SpawnPaint(MoveData move, int facing)
    {
        if (move.paintData.paintPrefab == null) return;

        var position = new Vector3(
            Entity.transform.position.x,
            Entity.transform.position.y,
            PaintManager.Instance.PaintZ
            );

        var offset = new Vector3(
            move.paintData.offsetPosition.x * -facing,
            move.paintData.offsetPosition.y,
            move.paintData.offsetPosition.z
            );

        var paint = Object.Instantiate(move.paintData.paintPrefab, position + offset, Quaternion.identity);
        var scale = move.paintData.paintScale;
        paint.transform.localScale = new Vector3(scale.x * -facing, scale.y, 1);

        var renderer = paint.GetComponent<Renderer>();
        var block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        block.SetColor("_BaseColor", Opponent.Get<EntityVFX>().PaintColor);
        renderer.SetPropertyBlock(block);
    }
}
