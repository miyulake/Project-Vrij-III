using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

/// <summary>
/// Class for ease of access to relevant input data.
/// </summary>
public class InputReader : MonoBehaviour
{
    private PlayerInput playerInput;

    // Gameplay input
    public bool ComboAttack    => playerInput.actions["ComboAttack"].triggered;
    public bool AttackForward  => playerInput.actions["AttackForward"].triggered;
    public bool AttackDownward => playerInput.actions["AttackDownward"].triggered;
    public bool AttackUpward   => playerInput.actions["AttackUpward"].triggered;
    public bool Blocking       => playerInput.actions["Block"].IsPressed();
    public bool Grabbing       => playerInput.actions["Grab"].triggered;
    public bool Snap           => playerInput.actions["Snap"].triggered;
    public bool Push           => playerInput.actions["Push"].triggered;
    public bool Taunt          => playerInput.actions["Taunt"].triggered;
    public Vector2 Movement    => playerInput.actions["Move"].ReadValue<Vector2>();

    // Menu input
    public bool Restart        => playerInput.actions["Restart"].triggered;
    public bool Pause          => playerInput.actions["Pause"].triggered;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        var gamepads = Gamepad.all;
        if (playerInput.playerIndex < gamepads.Count)
        {
            InputUser.PerformPairingWithDevice(gamepads[playerInput.playerIndex], playerInput.user);
        }
        playerInput.ActivateInput();
    }
}