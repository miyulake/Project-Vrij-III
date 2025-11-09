using Unity.VisualScripting;
using UnityEngine;

public class TwoDMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbodyTwoD;
    [SerializeField] private Animator animator;
    [Range(0, 10)] [SerializeField] private float baseSpeed = 5;
    [Range(0, 10)] [SerializeField] private float blockSpeed = 2;
    [Range(0, 500)] [SerializeField] private float acceleration = 50f;
    [Range(0, 100)] [SerializeField] private float deceleration = 50f;
    private InputReader inputReader;
    private Vector2 inputDirection;
    private Vector2 currentVelocity;

    private void Start() => inputReader = GetComponent<InputReader>();

    private void Update()
    {
        if (GameManager.Instance.MatchEnded)
        {
            rigidbodyTwoD.constraints = RigidbodyConstraints2D.FreezeAll;
            return; // HACK
        }

        inputDirection = CanMove() ? inputReader.Movement : Vector2.zero;
        Movement();
    }
    
    private void Movement()
    {
        var targetVelocity = inputDirection * GetSpeed();
        var accelerationRate = inputDirection.magnitude > 0 ? acceleration : deceleration;

        currentVelocity = Vector2.MoveTowards(rigidbodyTwoD.linearVelocity, targetVelocity, accelerationRate * Time.fixedDeltaTime);
        rigidbodyTwoD.linearVelocity = currentVelocity;
    }

    private bool CanMove() => !AnimatorUtils.IsInAnyState(animator, 
        AnimationHashes.Grab, 
        AnimationHashes.Stun, 
        AnimationHashes.BlockStun);

    private float GetSpeed() => animator.GetBool("IsBlocking") ? blockSpeed : baseSpeed;
}