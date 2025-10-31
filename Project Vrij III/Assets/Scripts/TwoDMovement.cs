using UnityEngine;

public class TwoDMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbodyTwoD;
    [SerializeField] private Animator animator;
    [Range(0, 10)] [SerializeField] private float baseSpeed = 5;
    [Range(0, 10)] [SerializeField] private float blockSpeed = 2;
    [Range(0, 100)] [SerializeField] private float acceleration = 50f;
    [Range(0, 100)] [SerializeField] private float deceleration = 50f;
    private InputReader inputReader;
    private Vector2 inputDirection;
    private Vector2 currentVelocity;

    private void Start() => inputReader = GetComponent<InputReader>();

    private void FixedUpdate()
    {
        inputDirection = CanMove() ? inputReader.Movement : Vector2.zero;
        GetMovement();
    }

    private void GetMovement()
    {
        var targetVelocity = inputDirection * GetSpeed();
        var accelerationRate = inputDirection.magnitude > 0 ? acceleration : deceleration;

        currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);
        currentVelocity = Vector2.ClampMagnitude(currentVelocity, GetSpeed());

        rigidbodyTwoD.linearVelocity = currentVelocity;
    }

    private bool CanMove() => 
        !AnimatorUtils.IsInAnyState(animator, AnimationHashes.Grab) ||
        !AnimatorUtils.IsInAnyState(animator, AnimationHashes.Stun) ||
        !AnimatorUtils.IsInAnyState(animator, AnimationHashes.BlockStun);

    private float GetSpeed() => animator.GetBool("IsBlocking") ? blockSpeed : baseSpeed;
}