using Game.Entities;
using UnityEngine;

public class TwoDMovement : EntityComponent, ITickable
{
    [Range(0, 10)] [SerializeField] private float baseSpeed = 5;
    [Range(0, 10)] [SerializeField] private float blockSpeed = 2;
    [Range(0, 500)] [SerializeField] private float acceleration = 50f;
    [Range(0, 100)] [SerializeField] private float deceleration = 50f;
    private Rigidbody2D m_RigidBodyTwoD;
    private Vector2 m_InputDirection;
    private Vector2 m_CurrentVelocity;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        m_RigidBodyTwoD = GetComponent<Rigidbody2D>();
    }

    public void Tick()
    {
        if (StateMachine.CurrentState is CaughtState || RoundManager.Instance.CurrentState == RoundState.INTRO)
        {
            m_RigidBodyTwoD.linearVelocity = Vector2.zero;
            return;
        }
        m_InputDirection = CanMove() ? Input.Movement : Vector2.zero;
        HandleMovement();
    }

    private void HandleMovement()
    {
        var targetVelocity = m_InputDirection * GetSpeed();
        var accelerationRate = m_InputDirection.magnitude > 0 ? acceleration : deceleration;

        m_CurrentVelocity = Vector2.MoveTowards(m_RigidBodyTwoD.linearVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);
        m_RigidBodyTwoD.linearVelocity = m_CurrentVelocity;
    }

    private bool CanMove() => StateMachine.CurrentState is DeadState ||
        !AnimatorUtils.IsInAnyState(AnimatorComp.GetAnimator(),
            AnimationHashes.Grab,
            AnimationHashes.Taunt,
            AnimationHashes.Stun,
            AnimationHashes.BlockStun);

    private float GetSpeed() => StateMachine.CurrentState is BlockState ? blockSpeed : baseSpeed;

    public Rigidbody2D GetRigidBody() => m_RigidBodyTwoD;
}