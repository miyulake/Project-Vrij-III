using Game.Entities;
using UnityEngine;

public class TwoDMovement : EntityComponent, ITickable
{
    public Rigidbody2D RigidBodyTwoD { get; private set; }
    private MovementSettings m_Settings;
    private Vector2 m_InputDirection;
    private float CurrentSpeed => StateMachine.CurrentState is BlockState ? m_Settings.blockSpeed : m_Settings.baseSpeed;

    public override void Initialize(Entity entity) 
    {
        base.Initialize(entity);
        RigidBodyTwoD = GetComponent<Rigidbody2D>();
        m_Settings = Entity.GetCharacter().GetMovement();
    }

    public void Tick()
    {
        if (StateMachine.CurrentState is CaughtState || RoundManager.Instance.CurrentState == RoundState.INTRO)
        {
            RigidBodyTwoD.linearVelocity = Vector2.zero;
            return;
        }
        m_InputDirection = CanMove() ? InputComp.Movement : Vector2.zero;
        HandleMovement();
    }

    private void HandleMovement()
    {
        var targetVelocity = m_InputDirection * CurrentSpeed;
        var accelerationRate = m_InputDirection.magnitude > 0 ? m_Settings.acceleration : m_Settings.deceleration;
        RigidBodyTwoD.linearVelocity = 
            Vector2.MoveTowards(RigidBodyTwoD.linearVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);
    }

    private bool CanMove() =>
        StateMachine.CurrentState is not DeadState &&
        !StateMachine.IsInStun() &&
        !AnimatorUtils.IsInAnyState(AnimatorComp.GetAnimator(), AnimationHashes.Grab, AnimationHashes.Taunt);
}