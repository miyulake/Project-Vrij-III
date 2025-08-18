using UnityEngine;
using UnityEngine.Events;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private UnityEvent onHitEvent;
    [SerializeField] private UnityEvent onStayEvent;
    [SerializeField] private UnityEvent onExitEvent;
    private AttackInfo currentAttackInfo;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox hurtbox)) return;
        hurtbox.ApplyHit(currentAttackInfo);
        onHitEvent.Invoke();
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

    public void SetAttackInfo(AttackInfo info) => currentAttackInfo = info;
}
