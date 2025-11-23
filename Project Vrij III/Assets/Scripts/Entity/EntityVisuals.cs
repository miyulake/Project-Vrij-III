using UnityEngine;

public class EntityVisuals : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] M_EyeMeshes;
    [SerializeField] private Material m_DeadMaterial;

    public void ChangeFaceMaterial()
    {
        for (int i = 0; i < M_EyeMeshes.Length; i++) M_EyeMeshes[i].sharedMaterial = m_DeadMaterial;
    }
}
