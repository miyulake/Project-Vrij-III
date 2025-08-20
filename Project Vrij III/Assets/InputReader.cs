using UnityEngine;

/// <summary>
/// Class for ease of acces to relevant player input data.
/// </summary>
public class InputReader : MonoBehaviour
{
    private Controls m_actions = null;

    public bool comboAttack => m_actions.Player.ComboAttack.triggered;
    public bool attackForward => m_actions.Player.AttackForward.triggered;
    public bool attackDownward => m_actions.Player.AttackDownward.triggered;
    public bool attackUpward => m_actions.Player.AttackUpward.IsPressed();
    public bool blocking     => m_actions.Player.Block.IsPressed();
    public Vector2 movement => m_actions.Player.Move.ReadValue<Vector2>();

    private void Awake() => m_actions = new Controls();

    private void Start() => m_actions.Enable();
}