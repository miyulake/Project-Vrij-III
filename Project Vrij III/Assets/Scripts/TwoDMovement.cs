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
        inputDirection = inputReader.Movement;
        GetMovement();
    }

    private void GetMovement()
    {
        var targetVelocity = inputDirection.normalized * GetSpeed();

        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            targetVelocity,
            (inputDirection != Vector2.zero ? acceleration : deceleration) * Time.fixedDeltaTime
            );

        rigidbodyTwoD.MovePosition(rigidbodyTwoD.position + currentVelocity * Time.fixedDeltaTime);
    }

    private float GetSpeed() => AnimatorUtils.IsInAnyState(animator, AnimationHashes.Block) ? blockSpeed : baseSpeed;
    // Should lock down movement and apply attack momentum, but not enough time
}