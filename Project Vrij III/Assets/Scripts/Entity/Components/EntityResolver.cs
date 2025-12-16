using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class EntityResolver
{
    public MoveData StoredMove { get; private set; }
    public ContactType HitType { get; private set; }
    public bool IsForced { get; private set; } = false;


    private readonly Entity m_Entity;
    public EntityResolver(Entity entity) => m_Entity = entity;

    public void ResolveHit(MoveData move)
    {
        if (move == null) return;

        var type = DetermineContact(move);
        var data = GetContactData(move, type);

        ApplyContact(data, type, move);
    }

    private ContactType DetermineContact(MoveData move)
    {
        var isBlocking = m_Entity.StateMachine.CurrentState is BlockState;
        var isAttacking = m_Entity.StateMachine.CurrentState is AttackState;
        var isRecovering = m_Entity.StateMachine.CurrentState is RecoverState;

        var unblockable = move.moveType == MoveType.GRAB || move.moveFlags == MoveFlags.UNBLOCKABLE;

        if (isAttacking) return ContactType.COUNTER;
        if (isRecovering) return ContactType.PUNISH;
        if (isBlocking && !unblockable) return ContactType.BLOCK;
        return ContactType.NORMAL;
    }

    private ContactData GetContactData(MoveData move, ContactType type)
    {
        HitType = type;

        return type switch
        {
            ContactType.NORMAL  => move.hit,
            ContactType.BLOCK   => move.block,
            ContactType.COUNTER => move.counterHit,
            ContactType.PUNISH  => move.hit,
            _                   => move.hit
        };
    }

    private void ApplyContact(ContactData data, ContactType type, MoveData move)
    {
        // Throw
        if (move.moveType == MoveType.GRAB && type != ContactType.COUNTER && !IsForced)
        {
            StoredMove = move;
            m_Entity.StateMachine.ChangeState<CaughtState>(false, move.breakFrames);
            return;
        }

        // Misc checks
        var usingPaint = GameManager.Instance.CurrentMode == GameMode.PAINT;
        var inGameplay = RoundManager.Instance.CurrentState == RoundState.GAMEPLAY;

        // Orientation
        var facingDirection = m_Entity.Orientation.FacingDirection;

        // States
        if (type == ContactType.BLOCK)
            m_Entity.StateMachine.ChangeState<BlockStunState>(false, data.stun);
        else
            m_Entity.StateMachine.ChangeState<HitStunState>(false, data.stun);

        // Knockback
        var knockback = data.knockback;
        knockback.x *= -facingDirection;
        m_Entity.Physics.ApplyKnockback(knockback);

        // VFX
        m_Entity.VFX.SpawnParticles(data);
        if (usingPaint && inGameplay) m_Entity.VFX.SpawnPaint(move, facingDirection);

        // Visuals
        var stunDuration = data.stun * Time.fixedDeltaTime;
        m_Entity.Shake.TriggerShake(stunDuration, data.shakeMagnitude);

        // Damage & Combo
        if (type != ContactType.BLOCK && !usingPaint)
        {
            var multiplier = m_Entity.Opponent.Taunt.GetMultiplier();
            var flatIncrease = m_Entity.Opponent.Taunt.GetFlatIncrease();
            var finalDamage = Mathf.RoundToInt((data.damage + flatIncrease) * multiplier);
            
            m_Entity.Combo.AddHit(finalDamage);
            m_Entity.Resources.ApplyDamage(finalDamage);
        }
        else if (usingPaint) m_Entity.Combo.AddHit(0);

        // Audio
        m_Entity.Audio.Play(data.sound);

        IsForced = false;
    }

    public void SetForceState(bool isForced) => IsForced = isForced;
}
