using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public StateManager State {  get; private set; }
    public Entity Entity { get; private set; }

    private void Awake()
    {
        State = GetComponent<StateManager>();
        Entity = GetComponent<Entity>();
    }
}
