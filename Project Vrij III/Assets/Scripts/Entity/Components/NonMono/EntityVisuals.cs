using Game.Entities;

public class EntityVisuals : EntityContext, IEntityComponent, IResettable
{
    private VisualsSettings m_Settings;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_Settings = Entity.Character.Visuals;
    }

    public void ApplyBaseVisuals() => ViewComp.Eyes.sharedMaterial = m_Settings.baseMaterial;
    public void ApplyDeadVisuals() => ViewComp.Eyes.sharedMaterial = m_Settings.deadMaterial;

    public void Reset() => ApplyBaseVisuals();
}