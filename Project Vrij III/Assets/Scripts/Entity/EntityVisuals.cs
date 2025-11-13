using UnityEngine;

public class EntityVisuals : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] M_EyeMeshes;
    [SerializeField] private Material m_DeadMaterial;
    private StateManager m_StateManager;

    private void Start() => m_StateManager = GetComponent<StateManager>();

    private void Update()
    {
        if (m_StateManager.CurrentState == EntityState.DEAD)
        {
            for (int i = 0; i < M_EyeMeshes.Length; i++) M_EyeMeshes[i].sharedMaterial = m_DeadMaterial;
        }
    }
}
