using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Entity entity;
    public AttackInfo AttackInfo { get; set; }

    private void Start() => entity = GetComponentInParent<Entity>();

    public void SetHitstunState(bool state) => entity.SetHitstunState(state);
    public void SetHitstunDuration() => entity.SetHitstunDuration(AttackInfo.hitstunDuration);
}
