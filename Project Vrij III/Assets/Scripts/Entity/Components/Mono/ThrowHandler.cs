using Game.Entities;
using UnityEngine;

public class ThrowHandler : EntityComponent, ITickable
{
    [SerializeField] MoveData m_ClankMove;
    [SerializeField] private GameObject m_ThrowAnchor;
    [SerializeField] private AudioClip m_CaughtSound;
    [SerializeField] private AudioClip m_ClankSound;
    private bool m_GrabConnected;

    public MoveData Clank        => m_ClankMove;
    public AudioClip CaughtSound => m_CaughtSound;
    public AudioClip ClankSound  => m_ClankSound;

    public void Tick() => HandleThrow();

    private void HandleThrow()
    {
        var shouldThrow = 
            m_ThrowAnchor.activeSelf && 
            m_GrabConnected && 
            ThrowEligible() && 
            RoundManager.Instance.CurrentState != RoundState.INTRO;

        if (shouldThrow)
        {
            ViewComp.EntityCollider.enabled = false;
            Opponent.transform.position = m_ThrowAnchor.transform.position;
        }
        else
        {
            ViewComp.EntityCollider.enabled = true;
            m_GrabConnected = false;
        }
    }

    public bool ThrowEligible()
    {
        var opponentSM = Opponent.Get<StateMachine>();
        return opponentSM.CurrentState is HitStunState || opponentSM.CurrentState is CaughtState;
    }

    public void ConnectGrab() => m_GrabConnected = true;
}