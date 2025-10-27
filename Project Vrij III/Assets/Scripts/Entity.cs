using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool PlayerOne { get; private set; } = true;
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

    private void Start()
    {
        movement = GetComponent<TwoDMovement>();
        shake = GetComponent<ShakeController>();
    }

    private void Update()
    {
        if (inHitstun) HandleHitstun();

        // Very bad and inefficient
        FlipCharacter();
    }

    private void HandleHitstun()
    {
        hitstunTimer += Time.deltaTime; // Get duration from the attack that the entity was hit with
        //FreezeEntity(true);

        if (hitstunTimer >= hitstunDuration)
        {
            inHitstun = false;
            //FreezeEntity(false);
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
            rigidbodyTwoD.AddForce(force, attackInfo.attackForceMode);

            // TEST
            if (attackInfo.paintPrefab != null) SpawnPaint(attackInfo);
        }
    }

    private void SpawnPaint(AttackInfo attackInfo)
    {
        var spawnPos = new Vector3(transform.position.x, transform.position.y, backgroundLayer.position.z);
        var paint = Instantiate
            (attackInfo.paintPrefab, spawnPos + attackInfo.offsetPosition, attackInfo.paintRotation, backgroundLayer);
        paint.transform.localScale = attackInfo.paintScale;
        // idk set material color I guess
        //paint.GetComponent<RawImage>().color = opponentColor; 
    }

    private void FlipCharacter()
    {
        if (PlayerOne)
        {
            if (transform.position.x > opponent.transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (transform.position.x < opponent.transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void FreezeEntity(bool freezeState) =>
        rigidbodyTwoD.constraints = freezeState
        ? RigidbodyConstraints2D.FreezeAll
        : RigidbodyConstraints2D.FreezeRotation;
}
