using Game.Entities;
using UnityEngine;

public class TwoDMovement : EntityContext, IEntityComponent, ITickable
{
    private MovementSettings m_Settings;
    private Vector2 m_InputDirection;
    private float CurrentSpeed => StateMachine.CurrentState is BlockState ? m_Settings.blockSpeed : m_Settings.baseSpeed;

    public void Initialize(Entity entity) 
    {
        SetEntity(entity);
        m_Settings = Entity.Character.GetMovement();
    }

    public void Tick()
    {
        if (StateMachine.CurrentState is CaughtState || RoundManager.Instance.CurrentState == RoundState.INTRO)
        {
            ViewComp.RigidBodyTwoD.linearVelocity = Vector2.zero;
            return;
        }
        m_InputDirection = CanMove() ? InputComp.Movement : Vector2.zero;
        HandleMovement();
    }

    private void HandleMovement()
    {
        var targetVelocity = m_InputDirection * CurrentSpeed;
        var accelerationRate = m_InputDirection.magnitude > 0 ? m_Settings.acceleration : m_Settings.deceleration;
        ViewComp.RigidBodyTwoD.linearVelocity = 
            Vector2.MoveTowards(ViewComp.RigidBodyTwoD.linearVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);
    }

    private bool CanMove() =>
        StateMachine.CurrentState is not DeadState &&
        !StateMachine.IsInStun() &&
        !AnimatorUtils.IsInAnyState(ViewComp.Animator, AnimationHashes.Grab, AnimationHashes.Taunt);
}