using Game.Entities;
using UnityEngine;
using System;

public class EntityResolver : EntityContext, IEntityComponent
{
    public event Action<ContactType> OnHitTypeChanged;
    public MoveData StoredMove { get; private set; }
    public bool IsForced { get; private set; } = false;

    public void Initialize(Entity entity) => SetEntity(entity);

    public void ResolveHit(MoveData move)
    {
        if (move == null) return;

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
        if (move.moveType == MoveType.GRAB && type != ContactType.COUNTER && !IsForced)
        {
            StoredMove = move;
            StateMachine.ChangeState<CaughtState>(false, move.breakFrames);
            return;
        }

        // Misc checks
        var usingPaint = GameManager.Instance.CurrentMode == GameMode.PAINT;
        var inGameplay = RoundManager.Instance.CurrentState == RoundState.GAMEPLAY;

        // Orientation
        var facingDirection = Orientation.FacingDirection;

        // States
        if (type == ContactType.BLOCK)
            StateMachine.ChangeState<BlockStunState>(false, data.stun);
        else
            StateMachine.ChangeState<HitStunState>(false, data.stun);

        // Knockback
        var knockback = data.knockback;
        knockback.x *= -facingDirection;
        Physics.ApplyKnockback(Movement.GetRigidBody(), knockback);

        // VFX
        VFX.SpawnParticles(data);
        if (usingPaint && inGameplay) VFX.SpawnPaint(move, facingDirection);

        // Visuals
        var stunDuration = data.stun * Time.fixedDeltaTime;
        Shake.TriggerShake(stunDuration, data.shakeMagnitude);

        // Damage & Combo
        if (type != ContactType.BLOCK && !usingPaint)
        {
            var oppnentTH = Entity.Opponent.Get<TauntHandler>();
            var multiplier = oppnentTH.GetMultiplier();
            var flatIncrease = oppnentTH.GetFlatIncrease();
            var finalDamage = Mathf.RoundToInt((data.damage + flatIncrease) * multiplier);
            
            if (!Combo.IsPaused) Combo.AddHit(finalDamage);
            Resources.ApplyDamage(finalDamage);
        }
        else if (usingPaint) Combo.AddHit(0);

        // Audio
        Audio.Play(data.sound);

        IsForced = false;
    }

    public void SetForceState(bool isForced) => IsForced = isForced;
}
