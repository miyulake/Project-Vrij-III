using UnityEngine;

public class TwoDMovement: MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbodyTwoDee;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    private Vector2 inputDirection;
    private Vector2 currentVelocity;
    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
        controls.Player.Move.performed += ctx => inputDirection = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => inputDirection = Vector2.zero;
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();


    private void FixedUpdate()
    {
        var targetVelocity = inputDirection.normalized * moveSpeed;

        currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity,
            (inputDirection != Vector2.zero ? acceleration : deceleration) * Time.fixedDeltaTime);

        rigidbodyTwoDee.MovePosition(rigidbodyTwoDee.position + currentVelocity * Time.fixedDeltaTime);
    }
}