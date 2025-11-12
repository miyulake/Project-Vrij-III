using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
    public int iD = 0;

    [SerializeField] private Animator m_Animator;
    [SerializeField] private Rigidbody2D m_RigidbodyTwoD;
    [SerializeField] private Transform m_Opponent, m_PaintLayer, m_ParticleSpawn;
    [SerializeField] private Color m_OpponentColor = Color.red;
    [SerializeField] private AudioSource m_FighterAudio;

    private InputReader m_InputReader;
    private StateManager m_StateManager;
    private ShakeController m_Shake;

    private float m_StunTimer;
    private float m_StunDuration;
    
    private int FacingDirection => 
        transform.position.x < m_Opponent.position.x ? 1 : -1; // Returns 1 or -1 depending on facing direction

    private void Start()
    {
        m_InputReader = GetComponent<InputReader>();
        m_StateManager = GetComponent<StateManager>();
        m_Shake = GetComponent<ShakeController>();
    }

    private void Update()
    {
        if (m_InputReader.Restart) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (m_StateManager.CurrentState == EntityState.DEAD) return;
        if (m_StateManager.IsInNeutral()) HandleBlock(m_InputReader.Blocking);
        if (m_StateManager.IsInStun()) HandleStun();
        UpdateFacingDirection();
        UpdateAnimator();
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
                ApplyHit(move.hit);
                SpawnPaint(move);
                m_Animator.Play("Stun", 0, 0);
                m_StateManager.SetState(EntityState.HITSTUN);
                break;

            case ContactType.BLOCK:
                ApplyHit(move.block);
                m_Animator.Play("Block_Stun", 0, 0);
                m_StateManager.SetState(EntityState.BLOCKSTUN);
                break;

            case ContactType.COUNTERHIT:
                ApplyHit(move.counterHit);
                SpawnPaint(move);
                m_Animator.Play("Stun");
                m_StateManager.SetState(EntityState.HITSTUN);
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

    private void ApplyHit(ContactData contact)
    {
        var duration = contact.stun / 60f;
        StartStun(duration);
        ApplyKnockback(contact);
        SpawnParticle(contact);
        m_Shake.TriggerShake(m_StunDuration, contact.shakeMagnitude);
        m_FighterAudio.PlayOneShot(contact.sound);
    }

    private void StartStun(float duration)
    {
        m_StunDuration = duration;
        m_StunTimer = 0f;
    }

    private void HandleStun()
    {
        m_StunTimer += Time.deltaTime;
        if (m_StunTimer >= m_StunDuration)
        {
            m_StunTimer = 0f;
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
        var block = new MaterialPropertyBlock();
        paint.GetComponent<Renderer>().GetPropertyBlock(block);
        block.SetColor("_BaseColor", m_OpponentColor);
        paint.GetComponent<Renderer>().SetPropertyBlock(block);
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