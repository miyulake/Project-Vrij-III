using UnityEngine;
using Game.Entities;
using System.Linq;

public class ThrowHandler : EntityContext, IEntityComponent, ITickable
{
    public MoveData Clank { get; private set; }
    private CapsuleCollider2D m_Collider;
    private bool m_GrabConnected;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_Collider = Entity.GetComponent<CapsuleCollider2D>();
        // Refactor this in the future
        Clank = Entity.Character.AllGenerics
            .FirstOrDefault(m => m.name == "MISC_Clank");
    }

    public void Tick() => HandleThrow();

    private void HandleThrow()
    {
        var throwAnchor = ViewComp.ThrowAnchor;

        var shouldThrow =
            throwAnchor.activeSelf &&
            m_GrabConnected &&
            ThrowEligible() &&
            RoundManager.Instance.CurrentState != RoundState.INTRO;

        if (shouldThrow)
        {
            m_Collider.enabled = false;
            Opponent.transform.position = throwAnchor.transform.position;
        }
        else
        {
            m_Collider.enabled = true;
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