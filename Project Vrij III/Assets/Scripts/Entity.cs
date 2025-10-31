using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbodyTwoD;
    [SerializeField] private Transform opponent;
    [SerializeField] private Transform backgroundLayer;
    [SerializeField] private Transform particleSpawn;
    [SerializeField] private Color opponentColor = Color.red;
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private AudioClip blockSound;
    private ShakeController shake;
    private bool inStun = false;
    private float stunDuration = 0f;
    private float stunTimer = 0f;

    private int FacingDirection
    {
        // Returns +1 or -1 depending on direction
        get { return transform.position.x < opponent.position.x ? 1 : -1; }
    }

    private void Start() => shake = GetComponent<ShakeController>();

    private void Update()
    {
        if (inStun) HandleStun();
        if (animator != null) UpdateFacingDirection();
    }

    private void HandleStun()
    {
        stunTimer += Time.deltaTime; // Get duration from the attack that the entity was hit with
        if (stunTimer >= stunDuration)
        {
            inStun = false;
            stunTimer = 0f;
        }
    }

    // We are reversing FacingDirection to get the direction of the attack which is always opposite
    public void ReceiveHit(AttackInfo attackInfo)
    {
        if (attackInfo == null || animator == null || rigidbodyTwoD == null) return;

        //var isBlocking = AnimatorUtils.IsInAnyState(animator, AnimationHashes.Block);
        var isBlocking = animator.GetBool("IsBlocking");
        var isGuardBreak = isBlocking && attackInfo.ignoresBlock;
        var isHit = !isBlocking || isGuardBreak;

        inStun = true;
        stunDuration = isHit ? attackInfo.hitStunDuration : attackInfo.blockStunDuration;
        stunTimer = 0f;

        var knockback = new Vector2(attackInfo.knockback.x * -FacingDirection, attackInfo.knockback.y);
        var appliedKnockback = isHit ? knockback : knockback * 0.1f;
        shake.TriggerShake(transform, stunDuration, attackInfo.shakeMagnitude);
        rigidbodyTwoD.AddForce(appliedKnockback, attackInfo.attackForceMode);

        if (attackInfo.paintPrefab != null && isHit) SpawnPaint(attackInfo);
        if (attackInfo.hitParticle != null && isHit) SpawnParticle(attackInfo);

        var stunAnimation = isHit ? "Stun" : "Block_Stun";
        animator.Play(stunAnimation, 0, 0);

        var impactSound = isHit ? attackInfo.hitSound : blockSound;
        playerAudio.PlayOneShot(impactSound);
    }

    private void SpawnPaint(AttackInfo attackInfo)
    {
        var position = new Vector3(
            transform.position.x,
            transform.position.y,
            backgroundLayer.position.z);
        var offset = new Vector3(
            attackInfo.offsetPosition.x * -FacingDirection,
            attackInfo.offsetPosition.y,
            attackInfo.offsetPosition.z);
        var scale = new Vector3(
            attackInfo.paintScale.x * -FacingDirection,
            attackInfo.paintScale.y,
            attackInfo.paintScale.z);
        var paint = Instantiate(attackInfo.paintPrefab, position + offset, Quaternion.identity, backgroundLayer);
        paint.transform.localScale = scale;

        // Set material color
        var block = new MaterialPropertyBlock();
        paint.GetComponent<Renderer>().GetPropertyBlock(block);
        block.SetColor("_BaseColor", opponentColor);
        paint.GetComponent<Renderer>().SetPropertyBlock(block);
    }

    private void SpawnParticle(AttackInfo attackInfo)
    {
        var scale = attackInfo.hitParticle.transform.localScale;
        var appliedScale = new Vector3(scale.x * -FacingDirection, scale.y, scale.z);
        var particle = Instantiate(attackInfo.hitParticle, particleSpawn);
        particle.transform.localScale = appliedScale;
    }

    private void UpdateFacingDirection()
    {
        if (AnimatorUtils.IsInAnyState(animator, AnimationHashes.Idle) ||
            AnimatorUtils.IsInAnyState(animator, AnimationHashes.Block))
            transform.localScale = new Vector3(FacingDirection, transform.localScale.y, transform.localScale.z);
    }

    private void FreezeEntity(bool freezeState) =>
        rigidbodyTwoD.constraints = freezeState
        ? RigidbodyConstraints2D.FreezeAll
        : RigidbodyConstraints2D.FreezeRotation;
}
