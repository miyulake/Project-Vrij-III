using UnityEngine;

public class Entity : MonoBehaviour
{
    public int iD = 0;
    public int CurrentHealth { get; private set; }
    public ContactType HitType { get; private set; }
    public int RecievedComboHits { get; private set; }
    public int RecievedComboDamage { get; private set; }
    [Space]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_RigidbodyTwoD;
    [SerializeField] private Transform m_ParticleSpawn;
    [SerializeField] private AudioSource m_FighterAudio;

    [Header("Paint Settings")]
    [SerializeField] private Transform m_PaintLayer;
    [SerializeField] private Color m_OpponentColor = Color.red;

    [Header("Turn Settings")]
    [SerializeField] private AnimationCurve m_TurnCurve;
    [Range(0.05f, 0.25f)] [SerializeField] private float m_TurnDuration = 0.2f;
    private int m_CurrentFacing = 1; 
    private float m_TurnTime = -1f;
    private float m_StartY;

    private EntityManager m_EntityManager;
    private StateManager m_StateManager;
    private InputReader m_InputReader;
    private ShakeController m_Shake;

    private int m_StunFrames;
    private int FacingDirection => 
        transform.position.x < m_EntityManager.OpponentTransform.position.x ? 1 : -1; // Returns 1 or -1 depending on facing direction

    private void Start()
    {
        m_EntityManager = GetComponent<EntityManager>();
        m_StateManager = GetComponent<StateManager>();
        m_InputReader = GetComponent<InputReader>();
        m_Shake = GetComponent<ShakeController>();
        CurrentHealth = GameManager.Instance.maxHealth;
    }

    private void FixedUpdate()
    {
        // Only go through logic if the game is still going or unpaused
        if (RoundManager.Instance.RoundEnded || GameManager.Instance.IsPaused()) return;

        TickLogic();
    }

    private void Update()
    {
        if (RoundManager.Instance.RoundEnded) return;
        CheckTurnNeeded();
        UpdateTurnRotation();
        UpdateAnimator();
    }

    private void TickLogic()
    {
        if (m_StateManager.IsInNeutral()) HandleBlock(m_InputReader.Blocking);
        if (m_StateManager.IsInStun()) HandleStun();
    }

    private void HandleBlock(bool isBlocking)
    {
        if (isBlocking)
            m_StateManager.SetState(EntityState.BLOCK);
        else if (m_StateManager.CurrentState == EntityState.BLOCK)
            m_StateManager.SetState(EntityState.IDLE);
    }

    public void ReceiveMove(MoveData move)
    {
        if (move == null) return;
        ApplyContact(move, CheckContactType(move));
    }

    private void ApplyContact(MoveData move, ContactType contactType)
    {
        HitType = contactType;

        ContactData contact = contactType switch
        {
            ContactType.NORMAL        => move.hit,
            ContactType.BLOCK         => move.block,
            ContactType.COUNTER       => move.counterHit,
            ContactType.PUNISH        => move.hit,
            _                         => move.hit
        };

        if (contactType == ContactType.BLOCK) m_StateManager.SetState(EntityState.BLOCKSTUN);
        else m_StateManager.SetState(EntityState.HITSTUN);

        ApplyHit(contact, contactType);

        if (GameManager.Instance.usePaint) SpawnPaint(move);
        else ApplyDamage(contact);
    }

    private ContactType CheckContactType(MoveData move)
    {
        var isBlocking = m_StateManager.CurrentState == EntityState.BLOCK || 
                             m_StateManager.CurrentState == EntityState.BLOCKSTUN;
        var isAttacking = m_StateManager.CurrentState == EntityState.ATTACK;
        var isRecovering = m_StateManager.CurrentState == EntityState.RECOVER;
        var isUnblockable = move.moveType == MoveType.GRAB || 
                                move.moveFlags == MoveFlags.UNBLOCKABLE;

        if (isAttacking) return ContactType.COUNTER;
        if (isRecovering) return ContactType.PUNISH;
        if (isBlocking && !isUnblockable) return ContactType.BLOCK;
        return ContactType.NORMAL;
    }

    private void ApplyHit(ContactData contact, ContactType type)
    {
        m_StunFrames = contact.stun;
        ApplyKnockback(contact);
        SpawnParticle(contact);

        var stunDuration = contact.stun * Time.fixedDeltaTime;
        m_Shake.TriggerShake(stunDuration, contact.shakeMagnitude);

        if (type != ContactType.BLOCK) SetComboInfo(contact);

        m_Animator.Play(type == ContactType.BLOCK ? "Block_Stun" : "Stun", 0, 0);
        m_FighterAudio.PlayOneShot(contact.sound);
    }

    private void HandleStun()
    {
        --m_StunFrames;
        if (m_StunFrames <= 0)
        {
            ResetComboInfo();
            // Check if player is still blocking after stun
            if (m_InputReader.Blocking) m_StateManager.SetState(EntityState.BLOCK);
            else m_StateManager.SetState(EntityState.IDLE);
        }
    }

    private void ApplyKnockback(ContactData contact)
    {
        // Copy the knockback data
        var knockback = contact.knockback;
        knockback.x *= -FacingDirection;

        // Reset any previous velocity
        m_RigidbodyTwoD.linearVelocity = Vector2.zero;
        m_RigidbodyTwoD.AddForce(knockback, ForceMode2D.Impulse);
    }

    private void ApplyDamage(ContactData contact)
    {
        CurrentHealth -= contact.damage;

        if (CurrentHealth <= 0)
        {
            m_StateManager.SetState(EntityState.DEAD);
            // We died so end the match
            RoundManager.Instance.EndRound();
        }
    }

    private Vector3 GetAdjustedScale(Vector3 scale) =>
        new(scale.x * -FacingDirection, scale.y, 1);

    private void SpawnPaint(MoveData move)
    {
        if (move.paintData.paintPrefab == null) return;

        var position = new Vector3(
            transform.position.x,
            transform.position.y,
            m_PaintLayer.position.z);
        var offset = new Vector3(
            move.paintData.offsetPosition.x * -FacingDirection,
            move.paintData.offsetPosition.y,
            move.paintData.offsetPosition.z);
        var paint = 
            Instantiate(move.paintData.paintPrefab, position + offset, Quaternion.identity, m_PaintLayer);
        paint.transform.localScale = GetAdjustedScale(move.paintData.paintScale);

        // Set material color
        var renderer = paint.GetComponent<Renderer>();
        var block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        block.SetColor("_BaseColor", m_OpponentColor);
        renderer.SetPropertyBlock(block);
    }

    private void SpawnParticle(ContactData contact)
    {
        if (contact.particleEffect == null) return;

        var scale = contact.particleEffect.transform.localScale;
        var appliedScale = new Vector3(scale.x /* * -FacingDirection */, scale.y, scale.z);
        var particle = Instantiate(contact.particleEffect, m_ParticleSpawn);
        particle.transform.localScale = appliedScale;
    }

    private void CheckTurnNeeded()
    {
        if (!m_StateManager.IsInNeutral()) return;

        if (FacingDirection != m_CurrentFacing)
        {
            m_CurrentFacing = FacingDirection;
            m_TurnTime = 0f;
            m_StartY = transform.localEulerAngles.y;
        }
    }

    private void UpdateTurnRotation()
    {
        if (m_TurnTime < 0f) return;

        m_TurnTime += Time.deltaTime;

        var time = Mathf.Clamp01(m_TurnTime / m_TurnDuration);
        var targetY = m_CurrentFacing == 1 ? 0f : 180f;
        var newY = Mathf.Lerp(m_StartY, targetY, m_TurnCurve.Evaluate(time));

        transform.localRotation = Quaternion.Euler(0f, newY, 0f);

        if (!m_StateManager.IsInNeutral())
        {
            m_CurrentFacing = FacingDirection;
            transform.localRotation = Quaternion.Euler(0f, m_CurrentFacing == 1 ? 0f : 180f, 0f);
            m_TurnTime = 1f;
        }

        if (time >= 1f) m_TurnTime = -1f;
    }

    private void UpdateAnimator()
    {
        m_Animator.SetBool("InStun", m_StateManager.IsInStun());
        m_Animator.SetBool("IsBlocking", m_StateManager.CurrentState == EntityState.BLOCK);
    }

    private void SetComboInfo(ContactData contact)
    {
        ++RecievedComboHits;
        RecievedComboDamage += contact.damage;
    }

    private void ResetComboInfo()
    {
        RecievedComboHits = 0;
        RecievedComboDamage = 0;
    }
}