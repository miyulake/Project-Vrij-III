using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] private TwoDMovement movement;
    [SerializeField] private ShakeController shake;
    private bool inHitstun = false;
    private float hitstunDuration = 0f;
    private float hitstunTimer = 0f;

    private void Update()
    {
        if (inHitstun) HandleHitstun();
    }

    private void HandleHitstun()
    {
        if (movement != null) movement.CanMove = false;
        
        hitstunTimer += Time.deltaTime; // Get duration from the attack that the entity was hit with
        if (hitstunTimer >= hitstunDuration)
        {
            inHitstun = false;
            if (movement != null) movement.CanMove = true;
            hitstunTimer = 0f;
        }
    }

    public void ReceiveHit(AttackInfo attackInfo)
    {
        inHitstun = true;
        hitstunDuration = attackInfo.hitstunDuration;
        hitstunTimer = 0f;

        if (movement != null) movement.CanMove = false;
        if (shake != null) shake.TriggerShake(transform, attackInfo.hitstunDuration, attackInfo.shakeMagnitude);    
    }
}
