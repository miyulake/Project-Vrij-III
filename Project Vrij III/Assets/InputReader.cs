using UnityEngine;

/// <summary>
/// Class for ease of access to relevant input data.
/// </summary>
public class InputReader : MonoBehaviour
{
    private Controls controls = null;

    // Player input
    public bool ComboAttack    => controls.Player.ComboAttack.triggered;
    public bool AttackForward  => controls.Player.AttackForward.triggered;
    public bool AttackDownward => controls.Player.AttackDownward.triggered;
    public bool AttackUpward   => controls.Player.AttackUpward.IsPressed();
    public bool Blocking       => controls.Player.Block.IsPressed();
    public bool Grabbing       => controls.Player.Grab.triggered;
    public bool Snap           => controls.Player.Snap.triggered;
    public Vector2 Movement    => controls.Player.Move.ReadValue<Vector2>();

    // Debug input
    public bool Restart => controls.Debug.Restart.triggered;

    private void Awake() => controls = new Controls();

    private void OnEnable() => controls.Enable();
    private void OnDisable () => controls.Disable();
}