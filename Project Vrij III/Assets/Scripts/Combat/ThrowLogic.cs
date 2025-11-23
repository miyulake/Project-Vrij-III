using UnityEngine;

public class ThrowLogic : MonoBehaviour
{
    [SerializeField] private GameObject m_ThrowAnchor;
    [SerializeField] private AnimationCurve m_TurnCurve;
    [Range(0.05f, 0.25f)] [SerializeField] private float m_TurnDuration = 0.2f;
    private EntityManager m_EntityManager;
    private CapsuleCollider2D m_PlayerCollider;
    private float m_TurnTime = -1f;
    private float m_StartY;

    private void Awake()
    {
        m_EntityManager = GetComponent<EntityManager>();
        m_PlayerCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update() => HandleThrow();

    private void HandleThrow()
    {
        if (m_ThrowAnchor.activeSelf && m_EntityManager.OpponentState.CurrentState == EntityState.HITSTUN)
        {
            if (m_TurnTime < 0f) m_TurnTime = 0f;

            m_PlayerCollider.enabled = false;
            m_EntityManager.OpponentTransform.position = m_ThrowAnchor.transform.position;
        }
        else
        {
            m_PlayerCollider.enabled = true;
            m_TurnTime = -1f;
        }
        UpdateRotation();
    }


    private void UpdateRotation()
    {
        if (m_TurnTime < 0f)
        {
            m_StartY = transform.localEulerAngles.y;
            return;
        }

        m_TurnTime += Time.deltaTime;

        var time = Mathf.Clamp01(m_TurnTime / m_TurnDuration);
        var newY = Mathf.Lerp(0f, 180f, m_TurnCurve.Evaluate(time));
        transform.localRotation = Quaternion.Euler(0f, m_StartY + newY, 0f);
    }
}