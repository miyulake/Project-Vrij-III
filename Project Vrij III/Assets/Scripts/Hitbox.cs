using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct AttackInfo
{
    public float strength;
    public float hitstunDuration;
}
public class Hitbox : MonoBehaviour
{
    [SerializeField] private UnityEvent onHitEvent;
    [SerializeField] private UnityEvent onStayEvent;
    [SerializeField] private UnityEvent onExitEvent;
    private Animator animator;

    private void Start() => animator = GetComponentInParent<Animator>();

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out Hurtbox hurtbox)) return;

        hurtbox.AttackInfo = GetAttackInfo(); // Get info first
        hurtbox.SetHitstunDuration();
        hurtbox.SetHitstunState(true);

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

    private AttackInfo GetAttackInfo()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        if (AttackDatabase.Data.TryGetValue(state, out var info)) return info;
        return new AttackInfo(); // Returns default values (0)
    }
}
