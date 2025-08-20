using UnityEngine;

public class TwoDMovement : MonoBehaviour
{
    public bool CanMove { get; set; } = true;
    [SerializeField] private Rigidbody2D rigidbodyTwoD;
    [SerializeField] private float baseSpeed = 5;
    [SerializeField] private float blockSpeed = 2;
    [Range(0, 100)] [SerializeField] private float acceleration = 10f;
    [Range(0, 100)] [SerializeField] private float deceleration = 10f;
    private StateManager stateManager;
    private InputReader inputReader;
    private Vector2 inputDirection;
    private Vector2 currentVelocity;

    private void Start()
    {
        stateManager = GetComponent<StateManager>();
        inputReader = GetComponent<InputReader>();
    }

    private void FixedUpdate()
    {
        if (CanMove && !stateManager.IsHitstun)
        {
            inputDirection = inputReader.movement;
            GetMovement();
        }
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

    private float GetSpeed() => stateManager.CurrentState == EntityState.BLOCKING ? blockSpeed : baseSpeed;
}