using Game.Entities;
using System.Linq;
using UnityEngine;

public class ThrowHandler : EntityComponent, ITickable
{
    public MoveData Clank { get; private set; }
    [SerializeField] private GameObject m_ThrowAnchor;
    private bool m_GrabConnected;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        // Refactor this in the future
        Clank = Entity.Character.AllGenerics
            .FirstOrDefault(m => m.name == "MISC_Clank");
    }

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
        return
            opponentSM.CurrentState is HitStunState ||
            opponentSM.CurrentState is CaughtState ||
            // A little buggy, maybe works after refactor
            opponentSM.CurrentState is DeadState;
    }

    public void ConnectGrab() => m_GrabConnected = true;
}