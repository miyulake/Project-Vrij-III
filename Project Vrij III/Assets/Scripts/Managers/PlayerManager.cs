using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public Entity playerOne;
    public Entity playerTwo;
    private readonly Entity[] m_All = new Entity[2];

    private void Awake()
    {
        Instance = this;
        playerOne.SetOpponent(playerTwo);
        playerTwo.SetOpponent(playerOne);
        m_All[0] = playerOne;
        m_All[1] = playerTwo;
    }

    public IReadOnlyList<Entity> All => m_All;
}
