using UnityEngine;

public class ThrowLogic : MonoBehaviour
{
    [SerializeField] private StateManager opponent;
    [SerializeField] private GameObject throwAnchor;
    private CapsuleCollider2D playerCollider;

    private void Start() => playerCollider = GetComponent<CapsuleCollider2D>();

    private void Update() => HandleThrow(); // REFACTOR THIS

    private void HandleThrow()
    {
        // Using gameObject to make sure the opponent doesn't teleport on player entering grab state
        if (throwAnchor.activeSelf && opponent.CurrentState == EntityState.HITSTUN)
        {
            playerCollider.enabled = false;
            opponent.transform.position = throwAnchor.transform.position;
        }
        else playerCollider.enabled = true;
    }
}