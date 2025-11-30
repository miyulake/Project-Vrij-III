using UnityEngine;

public class EntityVisuals : EntityComponent
{
    [SerializeField] private MeshRenderer[] m_EyeMeshes;
    [SerializeField] private Material m_NormalMaterial;
    [SerializeField] private Material m_DeadMaterial;
    
    public void SetNormalFace()
    {
        for (int i = 0; i < m_EyeMeshes.Length; i++) m_EyeMeshes[i].sharedMaterial = m_NormalMaterial;
    }

    public void SetDeadFace()
    {
        for (int i = 0; i < m_EyeMeshes.Length; i++) m_EyeMeshes[i].sharedMaterial = m_DeadMaterial;
    }

    public void Reset() => SetNormalFace();
}