using UnityEngine;
using Game.Entities;

public class EntityView : MonoBehaviour, IEntityComponent
{
    [SerializeField] private Rigidbody2D m_RigidBody2D;
    [SerializeField] private CapsuleCollider2D m_EntityCollider;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Transform m_Model;
    [SerializeField] private AudioSource m_AudioSource;

    public void Initialize(Entity entity) { }

    public Rigidbody2D RigidBodyTwoD          => m_RigidBody2D;
    public CapsuleCollider2D EntityCollider => m_EntityCollider;
    public Animator Animator                => m_Animator;
    public Transform Model                  => m_Model;
    public AudioSource AudioSource          => m_AudioSource;


}
