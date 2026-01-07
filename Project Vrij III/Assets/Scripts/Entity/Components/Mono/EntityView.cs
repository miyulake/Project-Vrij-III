using UnityEngine;
using Game.Entities;

public class EntityView : MonoBehaviour, IEntityComponent
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject m_TauntEffect, m_ThrowAnchor;
    [SerializeField] private Transform m_Model, m_ParticleSpawn;
    [SerializeField] private MeshRenderer m_Eyes;

    public void Initialize(Entity entity) { }

    public GameObject TauntEffect           => m_TauntEffect;
    public GameObject ThrowAnchor           => m_ThrowAnchor;
    public Transform Model                  => m_Model;
    public Transform ParticleSpawn          => m_ParticleSpawn;
    public Animator Animator                => m_Animator;
    public MeshRenderer Eyes                => m_Eyes;
}
