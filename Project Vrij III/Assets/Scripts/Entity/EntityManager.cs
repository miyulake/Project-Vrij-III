using UnityEngine;

public class EntityManager : MonoBehaviour
{
    // This
    public StateManager State {  get; private set; }
    public Entity Entity { get; private set; }
    
    // Opponent
    public EntityManager OpponentManager => PlayerManager.Instance.GetOpponent(this);
    public StateManager OpponentState => OpponentManager.State;
    public Entity Opponent => OpponentManager.Entity;
    public Transform OpponentTransform => Opponent.transform;

    private void Awake()
    {
        State = GetComponent<StateManager>();
        Entity = GetComponent<Entity>();
    }
}