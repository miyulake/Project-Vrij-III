using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public Entity playerOne;
    public Entity playerTwo;

    private void Awake()
    {
        Instance = this;
        playerOne.SetOpponent(playerTwo);
        playerTwo.SetOpponent(playerOne);
    }
}
