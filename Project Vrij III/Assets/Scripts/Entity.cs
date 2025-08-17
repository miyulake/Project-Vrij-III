using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private TwoDMovement movement;
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
        
        hitstunTimer += Time.deltaTime; // get duration from the attack that the entity was hit with
        if (hitstunTimer >= hitstunDuration)
        {
            inHitstun = false;
            if (movement != null) movement.CanMove = true;
            hitstunTimer = 0f;
        }
    }

    public bool SetHitstunState(bool state) => inHitstun = state;
    public float SetHitstunDuration(float duration) => hitstunDuration = duration;
}
