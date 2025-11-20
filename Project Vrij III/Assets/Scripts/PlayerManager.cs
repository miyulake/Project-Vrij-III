using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public EntityManager playerOne;
    public EntityManager playerTwo;

    private void Awake() => Instance = this;
}
