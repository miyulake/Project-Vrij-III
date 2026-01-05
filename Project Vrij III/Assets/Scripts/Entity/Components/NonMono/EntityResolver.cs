using Game.Entities;
using UnityEngine;
using System;

public class EntityResolver : EntityContext, IEntityComponent, IResettable
{
    public event Action<ContactType> OnHitTypeChanged;

    private MoveData m_StoredMove;
    private ContactType m_StoredContactType;
    private ContactData m_StoredContactData;

    public void Initialize(Entity entity) => SetEntity(entity);

    public void ResolveHit(MoveData move)
    {
        if (move == null || StateMachine.CurrentState is SuperState) return;

        var type = DetermineContact(move);
        var data = GetContactData(move, type);

        ApplyContact(data, type, move);
        if (type != ContactType.NORMAL) OnHitTypeChanged.Invoke(type);
    }

    private ContactType DetermineContact(MoveData move)
    {
        var isBlocking = StateMachine.CurrentState is BlockState;
        var isAttacking = StateMachine.CurrentState is AttackState;
        var isRecovering = StateMachine.CurrentState is RecoverState;

        var unblockable = 
            move.moveType == MoveType.GRAB || 
            move.moveFlags.HasFlag(MoveFlags.UNBLOCKABLE);

        if (isAttacking) return ContactType.COUNTER;
        if (isRecovering) return ContactType.PUNISH;
        if (isBlocking && !unblockable) return ContactType.BLOCK;
        return ContactType.NORMAL;
    }

    private ContactData GetContactData(MoveData move, ContactType type)
    {
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
        if (move.moveType == MoveType.GRAB && m_StoredMove == null)
        {
            m_StoredMove = move;
            m_StoredContactType = DetermineContact(move);
            m_StoredContactData = GetContactData(move, m_StoredContactType);

            var breakFrames = m_StoredContactType == ContactType.COUNTER ? 0 : move.breakFrames;
            StateMachine.ChangeState<CaughtState>(false, breakFrames);
            return;
        }

        // Game state checks
        var usingPaint = GameManager.Instance.CurrentMode == GameMode.PAINT;
        var inGameplay = RoundManager.Instance.CurrentState == RoundState.GAMEPLAY;

        // Orientation
        var facingDirection = OrientationComp.FacingDirection;

        // States
        if (type == ContactType.BLOCK)
            StateMachine.ChangeState<BlockStunState>(false, data.stun);
        else
            StateMachine.ChangeState<HitStunState>(false, data.stun);

        // Knockback
        var knockback = data.knockback;
        knockback.x *= -facingDirection;
        PhysicsComp.ApplyKnockback(ViewComp.RigidBodyTwoD, knockback);

        // VFX
        VFXComp.SpawnParticles(data);
        if (usingPaint && inGameplay) VFXComp.SpawnPaint(move, facingDirection);

        // Visuals
        var stunDuration = data.stun * Time.fixedDeltaTime;
        ShakeComp.TriggerShake(stunDuration, data.shakeMagnitude);

        // Damage & Combo
        if (type != ContactType.BLOCK && !usingPaint)
        {
            var oppnentEffects = Opponent.Get<EntityEffects>();
            var multiplier = oppnentEffects.GetMultiplier();
            var flatIncrease = oppnentEffects.GetFlatIncrease();
            var finalDamage = Mathf.RoundToInt((data.damage + flatIncrease) * multiplier);
            
            if (!ComboComp.IsPaused) ComboComp.AddHit(finalDamage);
            ResourcesComp.ApplyDamage(finalDamage);
        }
        else if (usingPaint) ComboComp.AddHit(0);

        // Audio
        AudioComp.Play(data.sound);
    }

    public void ApplyStoredMove()
    {
        ApplyContact(m_StoredContactData, m_StoredContactType, m_StoredMove);

        m_StoredMove = null;
        m_StoredContactType = ContactType.NORMAL;
        m_StoredContactData = null;
    }

    public void Reset()
    {
        m_StoredMove = null;
        m_StoredContactType = ContactType.NORMAL;
        m_StoredContactData = null;
    }
}
