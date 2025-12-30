using Game.Entities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : EntityContext, IEntityComponent
{
    public event Action PauseEvent;
    private PlayerInput m_PlayerInput;

    // Gameplay input
    public Vector2 Movement    => m_PlayerInput.actions["Move"].ReadValue<Vector2>();
    public bool ComboAttack    => m_PlayerInput.actions["ComboAttack"].triggered;
    public bool AttackForward  => m_PlayerInput.actions["AttackForward"].triggered;
    public bool AttackDownward => m_PlayerInput.actions["AttackDownward"].triggered;
    public bool AttackUpward   => m_PlayerInput.actions["AttackUpward"].triggered;
    public bool Block          => m_PlayerInput.actions["Block"].IsPressed();
    public bool Grab           => m_PlayerInput.actions["Grab"].triggered;
    public bool Snap           => m_PlayerInput.actions["Snap"].triggered;
    public bool Push           => m_PlayerInput.actions["Push"].triggered;
    public bool Taunt          => m_PlayerInput.actions["Taunt"].triggered;
    public bool Super          => m_PlayerInput.actions["Super"].triggered;

    // UI input
    public InputAction Pause   => m_PlayerInput.actions["Pause"];

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_PlayerInput = Entity.GetComponent<PlayerInput>();
        /*
        // Automatically pair gamepad if assigned
        var gamepads = Gamepad.all;
        if (m_PlayerInput.playerIndex < gamepads.Count)
            InputUser.PerformPairingWithDevice(gamepads[m_PlayerInput.playerIndex], m_PlayerInput.user);
        */
        Pause.performed += OnPausePerformed;
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        if (RoundManager.Instance.CurrentState == RoundState.KNOCKOUT) return;
        PauseEvent?.Invoke();
    }
}