using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private Entity entity;

    private void Start() => entity = GetComponentInParent<Entity>();

    public void SetHitstunState(bool state) => entity.SetHitstunState(state);
}
