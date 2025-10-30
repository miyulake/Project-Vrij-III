using UnityEngine;

/// <summary>
/// Class for ease of access to relevant input data.
/// </summary>
public class InputReader : MonoBehaviour
{
    private Controls controls = null;

    // Player input
    public bool ComboAttack    => controls.PlayerKeyboard.ComboAttack.triggered
                               || controls.PlayerController.ComboAttack.triggered;
    public bool AttackForward  => controls.PlayerKeyboard.AttackForward.triggered
                               || controls.PlayerController.AttackForward.triggered;
    public bool AttackDownward => controls.PlayerKeyboard.AttackDownward.triggered
                               || controls.PlayerController.AttackDownward.triggered;
    public bool AttackUpward   => controls.PlayerKeyboard.AttackUpward.IsPressed()
                               || controls.PlayerController.AttackUpward.IsPressed();
    public bool Blocking       => controls.PlayerKeyboard.Block.IsPressed()
                               || controls.PlayerController.Block.IsPressed();
    public bool Grabbing       => controls.PlayerKeyboard.Grab.triggered
                               || controls.PlayerController.Grab.triggered;
    public bool Snap           => controls.PlayerKeyboard.Snap.triggered
                               || controls.PlayerController.Snap.triggered;
    public Vector2 Movement => controls.PlayerKeyboard.Move.ReadValue<Vector2>();

    // Debug input
    public bool Restart => controls.Debug.Restart.triggered;

    private void Awake() => controls = new Controls();

    private void OnEnable() => controls.Enable();
    private void OnDisable () => controls.Disable();
}