using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public AttackManager Instance { get; private set; }
    private Controls controls;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        controls = new Controls();

        controls.Player.LightAttack.performed  += ctx => Attack(AttackType.LIGHT);
        controls.Player.MediumAttack.performed += ctx => Attack(AttackType.MEDIUM);
        controls.Player.HeavyAttack.performed  += ctx => Attack(AttackType.HEAVY);
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    public void Attack(AttackType type)
    {
        switch (type)
        {
            case AttackType.LIGHT:

                break;
            case AttackType.MEDIUM:

                break;
            case AttackType.HEAVY:

                break;
        }
        print("Used: " + type);
    }
}
