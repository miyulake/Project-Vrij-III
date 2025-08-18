using UnityEngine;

public class TwoDMovement : MonoBehaviour
{
    private StateManager stateManager;
    public bool CanMove { get; set; } = true;
    [SerializeField] private Rigidbody2D rigidbodyTwoDee;
    [SerializeField] private float baseSpeed = 5;
    [SerializeField] private float blockSpeed = 2;
    [Range(0, 100)] [SerializeField] private float acceleration = 10f;
    [Range(0, 100)] [SerializeField] private float deceleration = 10f;
    private Vector2 inputDirection;
    private Vector2 currentVelocity;
    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
        controls.Player.Move.performed += ctx => inputDirection = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => inputDirection = Vector2.zero;
    }

    private void Start()
    {
        stateManager = GetComponent<StateManager>();
        //stateManager.OnStateChanged += GetSpeed();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void FixedUpdate()
    {
        if (CanMove) GetMovement();
    }

    private void GetMovement()
    {
        var targetVelocity = inputDirection.normalized * GetSpeed();
        currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity,
            (inputDirection != Vector2.zero ? acceleration : deceleration) * Time.fixedDeltaTime);
        rigidbodyTwoDee.MovePosition(rigidbodyTwoDee.position + currentVelocity * Time.fixedDeltaTime);
    }

    private float GetSpeed() => stateManager.CurrentState == EntityState.BLOCKING ? blockSpeed : baseSpeed;
}