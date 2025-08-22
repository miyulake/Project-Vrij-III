using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbodyTwoD;
    private TwoDMovement movement;
    private ShakeController shake;
    private bool inHitstun = false;
    private float hitstunDuration = 0f;
    private float hitstunTimer = 0f;

    private void Start()
    {
        movement = GetComponent<TwoDMovement>();
        shake = GetComponent<ShakeController>();
    }

    private void Update()
    {
        if (inHitstun) HandleHitstun();
    }

    private void HandleHitstun()
    {
        hitstunTimer += Time.deltaTime; // Get duration from the attack that the entity was hit with
        if (hitstunTimer >= hitstunDuration)
        {
            inHitstun = false;
            if (movement != null) FreezeEntity(false);
            hitstunTimer = 0f;
        }
    }

    public void ReceiveHit(AttackInfo attackInfo)
    {
        //AnimatorUtils.IsInAnyState(animator, AnimationHashes.Stun);

        inHitstun = true;
        hitstunDuration = attackInfo.hitstunDuration;
        hitstunTimer = 0f;

        // IT SHOULD FIRST APPLY THE HITSTUN SHAKE AND THEN APPLY FORCE TO THE RIGIDBODY!
        //FreezeEntity(true); 
        if (shake != null) shake.TriggerShake(transform, attackInfo.hitstunDuration, attackInfo.shakeMagnitude);    
        if (rigidbodyTwoD != null)
        {
            var force = new Vector3(attackInfo.knockback.x, attackInfo.knockback.y);
            rigidbodyTwoD.AddForce(force, attackInfo.forceMode);
        }
    }

    private void FreezeEntity(bool freezeState) =>
        rigidbodyTwoD.constraints = freezeState
        ? RigidbodyConstraints2D.FreezeAll
        : RigidbodyConstraints2D.FreezeRotation;
}
