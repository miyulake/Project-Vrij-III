using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool isBlocking = false; // Test variable
    [SerializeField] private bool PlayerOne = true;
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
        get { return (PlayerOne ^ (transform.position.x > opponent.position.x)) ? 1 : -1; }
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

    public void ReceiveHit(AttackInfo attackInfo)
    {
        //var isBlocking = AnimatorUtils.IsInAnyState(animator, AnimationHashes.Block);

        inStun = true;
        stunDuration = isBlocking ? attackInfo.blockStunDuration : attackInfo.hitStunDuration;
        stunTimer = 0f;

        var force = new Vector3(attackInfo.knockback.x * FacingDirection, attackInfo.knockback.y);
        var appliedForce = isBlocking ? force * 0.1f : force;
        var impactSound = isBlocking ? blockSound : attackInfo.hitSound;

        shake.TriggerShake(transform, stunDuration, attackInfo.shakeMagnitude);
        rigidbodyTwoD.AddForce(appliedForce, attackInfo.attackForceMode);
        if (attackInfo.paintPrefab != null && !isBlocking) SpawnPaint(attackInfo);
        if (attackInfo.hitParticle != null && !isBlocking) SpawnParticle(attackInfo);
        playerAudio.PlayOneShot(impactSound);
    }

    private void SpawnPaint(AttackInfo attackInfo)
    {
        var position = new Vector3(
            transform.position.x,
            transform.position.y,
            backgroundLayer.position.z);
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

        // Set material color
        var block = new MaterialPropertyBlock();
        paint.GetComponent<Renderer>().GetPropertyBlock(block);
        block.SetColor("_BaseColor", opponentColor);
        paint.GetComponent<Renderer>().SetPropertyBlock(block);
    }

    private void SpawnParticle(AttackInfo attackInfo)
    {
        var scale = attackInfo.hitParticle.transform.localScale;
        var appliedScale = new Vector3(scale.x * FacingDirection, scale.y, scale.z);
        var particle = Instantiate(attackInfo.hitParticle, particleSpawn);
        particle.transform.localScale = appliedScale;
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
