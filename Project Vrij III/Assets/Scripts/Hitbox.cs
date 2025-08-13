using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private UnityEvent onHitEvent;
    [SerializeField] private UnityEvent onStayEvent;
    [SerializeField] private UnityEvent onExitEvent;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox hurtbox)) return;

        hurtbox.SetHitstunState(true); // Maybe instead use a function that handles all the hitstun stuff
        onHitEvent.Invoke();
        print(gameObject.name + ": hit hurtbox");
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox _)) return;
        onStayEvent.Invoke();
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox _)) return;
        onExitEvent.Invoke();
    }
}
