using UnityEngine;

public class EntityOld : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbodyTwoD;
    [SerializeField] private Transform opponent, paintLayer, particleSpawn;
    [SerializeField] private Color opponentColor = Color.red;
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private AudioClip blockSound;
    private ShakeController shake;
    private bool inStun = false;
    private bool guardBroken = false;
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
        animator.SetBool("InStun", inStun);
    }

    private void HandleStun()
    {
        animator.SetBool("IsBlocking", !guardBroken); // TEST HACK
        stunTimer += Time.deltaTime;
        if (stunTimer >= stunDuration)
        {
            guardBroken = false; // TEST HACK
            inStun = false;
            stunTimer = 0f;
        }
    }

    // We are reversing FacingDirection to get the direction of the attack which is always opposite
    public void ReceiveHit(AttackInfo attackInfo)
    {
        if (attackInfo == null || animator == null || rigidbodyTwoD == null) return;

        var isBlocking = animator.GetBool("IsBlocking");
        var isGuardBreak = isBlocking && attackInfo.ignoresBlock;
        var isHit = !isBlocking || isGuardBreak;

        guardBroken = isHit; // TEST HACK

        inStun = true;
        stunDuration = isHit ? attackInfo.hitStunDuration : attackInfo.blockStunDuration;
        stunTimer = 0f;

        var knockback = new Vector2(attackInfo.knockback.x * -FacingDirection, attackInfo.knockback.y);
        if (!isHit) knockback *= 0.5f; // Half knockback on block

        rigidbodyTwoD.linearVelocity = Vector2.zero; // reset any previous velocity
        rigidbodyTwoD.AddForce(knockback, attackInfo.attackForceMode);

        shake.TriggerShake(stunDuration, attackInfo.shakeMagnitude);

        if (attackInfo.paintPrefab != null && isHit) SpawnPaint(attackInfo);
        if (attackInfo.hitParticle != null && isHit) SpawnParticle(attackInfo);

        animator.Play(isHit ? "Stun" : "Block_Stun", 0, 0);
        playerAudio.PlayOneShot(isHit ? attackInfo.hitSound : blockSound);
    }


    private void SpawnPaint(AttackInfo attackInfo)
    {
        var position = new Vector3(
            transform.position.x,
            transform.position.y,
            paintLayer.position.z);
        var offset = new Vector3(
            attackInfo.offsetPosition.x * -FacingDirection,
            attackInfo.offsetPosition.y,
            attackInfo.offsetPosition.z);
        var scale = new Vector3(
            attackInfo.paintScale.x * -FacingDirection,
            attackInfo.paintScale.y,
            attackInfo.paintScale.z);
        var paint = Instantiate(attackInfo.paintPrefab, position + offset, Quaternion.identity, paintLayer);
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
