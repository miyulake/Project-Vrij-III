using UnityEngine;

public class Entity : MonoBehaviour
{
    private bool inHitstun = false;
    private float hitstunTimer = 0f;

    private void Update()
    {
        if (inHitstun) HandleHitstun();
    }

    private void HandleHitstun()
    {
        // get duration from the attack that the entity was hit with
        hitstunTimer += Time.deltaTime;
        //if (hitstunTimer >= hitstunDuration)
    }

    public bool SetHitstunState(bool state) => inHitstun = state;
}
