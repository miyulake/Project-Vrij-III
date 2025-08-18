using UnityEngine;

/// <summary>
/// Class for ease of acces to relevant player input data.
/// </summary>
public class InputReader : MonoBehaviour
{
    private PlayerInputActions m_actions = null;

    //  Combat:
    public bool lightAttack => m_actions.Player.LightAttack.triggered;
    public bool heavyAttack => m_actions.Player.HeavyAttack.triggered;
    public bool dashing     => m_actions.Player.Dash.triggered;

    public bool holdingHeavyAttack => m_actions.Player.HeavyAttack.IsPressed();

    //  Combat:
    public Vector2 movement => m_actions.Player.Movement.ReadValue<Vector2>();
    public Vector2 looking
    {
        get
        {
            var mouse       = 0.1f * m_actions.Player.MouseLooking.ReadValue<Vector2>();
            var joystick    = m_actions.Player.JoystickLooking.ReadValue<Vector2>();

            return  mouse + joystick;
        }
    }

    private void Awake()
    {
        m_actions = new PlayerInputActions();
    }

    private void Start()
    {
        m_actions.Enable();
    }
}
