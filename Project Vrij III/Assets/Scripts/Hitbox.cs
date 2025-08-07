using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private UnityEvent onHitEvent;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox _)) return;

        onHitEvent.Invoke();
        print(gameObject.name + ": hit hurtbox");
    }
}
