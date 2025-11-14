using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public int iD = 0;
    [Space]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_RigidbodyTwoD;
    [SerializeField] private Transform m_Opponent, m_ParticleSpawn;
    [SerializeField] private Slider m_HealthBar;
    [SerializeField] private AudioSource m_FighterAudio;

    [Header("Paint Settings")]
    [SerializeField] private Transform m_PaintLayer;
    [SerializeField] private Color m_OpponentColor = Color.red;

    [Header("Health Settings")]
    [SerializeField] private int m_MaxHealth = 100;
    private int m_CurrentHealth;

    private InputReader m_InputReader;
    private StateManager m_StateManager;
    private ShakeController m_Shake;

    private int m_StunFrames;
    private int FacingDirection => 
        transform.position.x < m_Opponent.position.x ? 1 : -1; // Returns 1 or -1 depending on facing direction

    private void Start()
    {
        m_InputReader = GetComponent<InputReader>();
        m_StateManager = GetComponent<StateManager>();
        m_Shake = GetComponent<ShakeController>();
        m_CurrentHealth = m_MaxHealth;
        m_HealthBar.value = m_MaxHealth;
    }

    private void FixedUpdate()
    {
        // Only go through logic if the game is unpaused
        if (GameManager.Instance.IsPaused()) return;

        TickLogic();
        if (m_StateManager.IsInNeutral()) HandleBlock(m_InputReader.Blocking);
    }

    private void Update()
    {
        if (m_InputReader.Restart) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (GameManager.Instance.MatchEnded) return;
        UpdateFacingDirection();
        UpdateAnimator();
    }

    private void TickLogic()
    {
        if (GameManager.Instance.MatchEnded) return;
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
        switch (contactType)
        {
            case ContactType.HIT:
                m_StateManager.SetState(EntityState.HITSTUN);
                ApplyHit(move.hit, contactType);
                
                if (GameManager.Instance.usePaint) SpawnPaint(move);
                else ApplyDamage(move.hit);
                break;

            case ContactType.BLOCK:
                m_StateManager.SetState(EntityState.BLOCKSTUN);
                ApplyHit(move.block, contactType);

                if (!GameManager.Instance.usePaint) ApplyDamage(move.block);
                break;

            case ContactType.COUNTERHIT:
                m_StateManager.SetState(EntityState.HITSTUN);
                ApplyHit(move.counterHit, contactType);

                if (GameManager.Instance.usePaint) SpawnPaint(move);
                else ApplyDamage(move.counterHit);
                break;
        }
    }

    private ContactType CheckContactType(MoveData move)
    {
        var isBlocking = m_StateManager.CurrentState == EntityState.BLOCK || 
            m_StateManager.CurrentState == EntityState.BLOCKSTUN;
        var isAttacking = m_StateManager.CurrentState == EntityState.ATTACK;
        var isUnblockable = move.moveType == MoveType.GRAB || move.moveFlags == MoveFlags.UNBLOCKABLE;

        if (isAttacking) return ContactType.COUNTERHIT;
        if (isBlocking && !isUnblockable) return ContactType.BLOCK;
        return ContactType.HIT;
    }

    private void ApplyHit(ContactData contact, ContactType type)
    {
        m_StunFrames = contact.stun;
        ApplyKnockback(contact);
        SpawnParticle(contact);

        var stunDuration = contact.stun * Time.fixedDeltaTime;
        m_Shake.TriggerShake(stunDuration, contact.shakeMagnitude);

        m_Animator.Play(type == ContactType.BLOCK ? "Block_Stun" : "Stun", 0, 0);
        m_FighterAudio.PlayOneShot(contact.sound);
    }

    private void HandleStun()
    {
        --m_StunFrames;
        if (m_StunFrames <= 0)
        {
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
        m_CurrentHealth -= contact.damage;
        m_HealthBar.value = m_CurrentHealth;

        if (m_CurrentHealth <= 0)
        {
            m_StateManager.SetState(EntityState.DEAD);
            // We died so end the match
            GameManager.Instance.EndMatch();
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
        var appliedScale = new Vector3(scale.x * -FacingDirection, scale.y, scale.z);
        var particle = Instantiate(contact.particleEffect, m_ParticleSpawn);
        particle.transform.localScale = appliedScale;
    }

    private void UpdateFacingDirection()
    {
        if (m_StateManager.IsInNeutral()) transform.localScale = 
                new Vector3(FacingDirection, transform.localScale.y, transform.localScale.z);
    }

    private void UpdateAnimator()
    {
        m_Animator.SetBool("InStun", m_StateManager.IsInStun());
        m_Animator.SetBool("IsBlocking", m_StateManager.CurrentState == EntityState.BLOCK);
    }
}