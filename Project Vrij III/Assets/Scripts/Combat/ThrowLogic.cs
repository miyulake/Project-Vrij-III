using UnityEngine;

public class ThrowLogic : MonoBehaviour
{
    [SerializeField] private GameObject throwAnchor;
    [SerializeField] private Transform opponent;
    private CapsuleCollider2D playerCollider;
    private Animator opponentAnimator;
    private Animator animator;

    private void Start() 
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        opponentAnimator = opponent.gameObject.GetComponentInChildren<Animator>();
    } 

    private void Update()
    {
        HandleThrow(); // The worst solution of all time award
    }

    private void HandleThrow()
    {
        // Using gameObject to make sure the opponent doesn't teleport on player entering grab state
        if (throwAnchor.activeSelf && AnimatorUtils.IsInAnyState(opponentAnimator, AnimationHashes.Stun))
        {
            playerCollider.enabled = false;
            opponent.position = throwAnchor.transform.position;
        }
        else playerCollider.enabled = true;
    }
}
