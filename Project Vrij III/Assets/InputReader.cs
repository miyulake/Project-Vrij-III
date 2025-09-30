using UnityEngine;

/// <summary>
/// Class for ease of acces to relevant player input data.
/// </summary>
public class InputReader : MonoBehaviour
{
    private Controls controls = null;

    public bool ComboAttack => controls.Player.ComboAttack.triggered;
    public bool AttackForward => controls.Player.AttackForward.triggered;
    public bool AttackDownward => controls.Player.AttackDownward.triggered;
    public bool AttackUpward => controls.Player.AttackUpward.IsPressed();
    public bool Blocking => controls.Player.Block.IsPressed();
    public Vector2 Movement => controls.Player.Move.ReadValue<Vector2>();

    private void Awake() => controls = new Controls();

    private void Start() => controls.Enable();
}