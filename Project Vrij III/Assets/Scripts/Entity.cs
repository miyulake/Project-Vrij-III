using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private bool PlayerOne = true;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbodyTwoD;
    [SerializeField] private Transform opponent;
    [SerializeField] private Transform backgroundLayer;
    [SerializeField] private Color playerColor = Color.red;
    private TwoDMovement movement;
    private ShakeController shake;
    private bool inHitstun = false;
    private float hitstunDuration = 0f;
    private float hitstunTimer = 0f;

    private int FacingDirection
    {
        // Returns +1 or -1 depending on direction
        get { return (PlayerOne ^ (transform.position.x > opponent.position.x)) ? 1 : -1; }
    }

    private void Start()
    {
        movement = GetComponent<TwoDMovement>();
        shake = GetComponent<ShakeController>();
    }

    private void Update()
    {
        if (inHitstun) HandleHitstun();
        if (animator != null) UpdateFacingDirection();
    }

    private void HandleHitstun()
    {
        hitstunTimer += Time.deltaTime; // Get duration from the attack that the entity was hit with

        if (hitstunTimer >= hitstunDuration)
        {
            inHitstun = false;
            hitstunTimer = 0f;
        }
    }

    public void ReceiveHit(AttackInfo attackInfo)
    {
        //AnimatorUtils.IsInAnyState(animator, AnimationHashes.Stun);

        inHitstun = true;
        hitstunDuration = attackInfo.hitstunDuration;
        hitstunTimer = 0f;

        if (shake != null) shake.TriggerShake(transform, attackInfo.hitstunDuration, attackInfo.shakeMagnitude);
        if (rigidbodyTwoD != null)
        {
            var force = new Vector3(attackInfo.knockback.x * FacingDirection, attackInfo.knockback.y);
            rigidbodyTwoD.AddForce(force, attackInfo.attackForceMode);

            if (attackInfo.paintPrefab != null) SpawnPaint(attackInfo);
        }
    }

    private void SpawnPaint(AttackInfo attackInfo)
    {
        var position = new Vector3(transform.position.x, transform.position.y, backgroundLayer.position.z);
        var offset = new Vector3(
            attackInfo.offsetPosition.x * FacingDirection, 
            attackInfo.offsetPosition.y,
            attackInfo.offsetPosition.z);
        var scale = new Vector3(
            attackInfo.paintScale.x * FacingDirection,
            attackInfo.paintScale.y,
            attackInfo.paintScale.z);
        var paint = Instantiate(attackInfo.paintPrefab, position + offset, attackInfo.paintRotation, backgroundLayer);
        paint.transform.localScale = scale;

        // idk set material color I guess
        //paint.GetComponent<RawImage>().color = opponentColor; 
    }

    private void UpdateFacingDirection()
    {
        if (Mathf.Sign(transform.localScale.x) != FacingDirection && AnimatorUtils.IsInAnyState(animator, AnimationHashes.Idle))
            transform.localScale = new Vector3(FacingDirection, transform.localScale.y, transform.localScale.z);
    }

    private void FreezeEntity(bool freezeState) =>
        rigidbodyTwoD.constraints = freezeState
        ? RigidbodyConstraints2D.FreezeAll
        : RigidbodyConstraints2D.FreezeRotation;
}
