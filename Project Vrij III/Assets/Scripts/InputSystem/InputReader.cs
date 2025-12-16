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
    public Vector2 Movement    => playerInput.actions["Move"].ReadValue<Vector2>();
    public bool ComboAttack    => playerInput.actions["ComboAttack"].triggered;
    public bool AttackForward  => playerInput.actions["AttackForward"].triggered;
    public bool AttackDownward => playerInput.actions["AttackDownward"].triggered;
    public bool AttackUpward   => playerInput.actions["AttackUpward"].triggered;
    public bool Block          => playerInput.actions["Block"].IsPressed();
    public bool Grab           => playerInput.actions["Grab"].triggered;
    public bool Snap           => playerInput.actions["Snap"].triggered;
    public bool Push           => playerInput.actions["Push"].triggered;
    public bool Taunt          => playerInput.actions["Taunt"].triggered;
    public bool Super          => playerInput.actions["Super"].triggered;

    // UI input
    public bool Pause          => playerInput.actions["Pause"].triggered;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        var gamepads = Gamepad.all;
        if (playerInput.playerIndex < gamepads.Count)
            InputUser.PerformPairingWithDevice(gamepads[playerInput.playerIndex], playerInput.user);

        playerInput.ActivateInput();
    }
}