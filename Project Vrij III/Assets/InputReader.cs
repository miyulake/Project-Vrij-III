using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class for ease of access to relevant input data.
/// </summary>
public class InputReader : MonoBehaviour
{
    private PlayerInput playerInput;

    // Player input
    public bool ComboAttack    => playerInput.actions["ComboAttack"].triggered;
    public bool AttackForward  => playerInput.actions["AttackForward"].triggered;
    public bool AttackDownward => playerInput.actions["AttackDownward"].triggered;
    public bool AttackUpward   => playerInput.actions["AttackUpward"].IsPressed();
    public bool Blocking       => playerInput.actions["Block"].IsPressed();
    public bool Grabbing       => playerInput.actions["Grab"].triggered;
    public bool Snap           => playerInput.actions["Snap"].triggered;
    public Vector2 Movement    => playerInput.actions["Move"].ReadValue<Vector2>();

    // Debug input
    public bool Restart => playerInput.actions["Restart"].triggered;

    private void Awake() => playerInput = GetComponent<PlayerInput>();
}