using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbodyTwoD;

    // THIS SHOULD HAPPEN VIA THE HITBOX/ATTACKINFO
    [SerializeField] private GameObject splatterPrefab;
    [SerializeField] private Transform backgroundLayer;
    [SerializeField] private Color opponentColor;
    //

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

            // TEST
            SpawnPaint(Vector2.zero);
        }
    }

    public void SpawnPaint(Vector3 hitPosition)
    {
        var spawnPos = new Vector3(hitPosition.x, hitPosition.y, backgroundLayer.position.z);
        var paint = Instantiate(splatterPrefab, spawnPos, Quaternion.identity, backgroundLayer);
        //paint.GetComponent<RawImage>().color = opponentColor; // idk set material color I guess

        // This shouldn't be random and get assigned in the attack info
        //splat.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));a
        //splat.transform.localScale *= Random.Range(0.8f, 1.2f);
    }

    private void FreezeEntity(bool freezeState) =>
        rigidbodyTwoD.constraints = freezeState
        ? RigidbodyConstraints2D.FreezeAll
        : RigidbodyConstraints2D.FreezeRotation;
}
