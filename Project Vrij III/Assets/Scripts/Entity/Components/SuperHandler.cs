using Game.Entities;
using UnityEngine;

public class SuperHandler : EntityComponent, ITickable
{
    [SerializeField] private SuperType m_CurrentSuper;
    [SerializeField] private GameObject m_Aura;
    [SerializeField] private AnimationCurve m_AuraCurve;
    [SerializeField] private float m_AuraCurveDuration = 1f;
    private SuperData[] m_AllSupers;
    private Vector3 m_OriginalAuraSize;
    private float m_AuraCurveTime = 0;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        m_AllSupers = Resources.LoadAll<SuperData>("MoveData");
        m_OriginalAuraSize = m_Aura.transform.localScale;
        m_Aura.transform.localScale = Vector3.zero;
    }

    public void Tick()
    {
        if (RoundManager.Instance.CurrentState != RoundState.GAMEPLAY) return;
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1)) ResourcesComp.AddMeter(GameManager.Instance.GetMaxMeter());
#endif
        UpdateAura();
        Debug.Log($"Meter before super: {ResourcesComp.Meter.Current}");
        var currentSuper = GetSuperData();
        if (InputComp.Super && currentSuper.CanExecute(ResourcesComp) && IsInSuperCondition(currentSuper))
            ExecuteSuper(currentSuper);
    }

    private void ExecuteSuper(SuperData data)
    {
        switch (data.superType)
        {
            case SuperType.CHAIN:
                //ActivateChain();
                break;
            case SuperType.TIME:
                //ActivateTime();
                break;
            case SuperType.TAUNT:
                ActivateTaunt();
                break;
        }
        AttackComp.StartMove(data);
        StateMachine.ChangeState<SuperState>(false, data.freezeFrames, data.activationFrames);
        ResourcesComp.Meter.Modify(-data.meterCost);
        Debug.Log($"Meter after super: {ResourcesComp.Meter.Current}");
    }

    private bool IsInSuperCondition(SuperData data)
    {
        return data.superType switch
        {
            // TESTING VALUES FOR NOW
            SuperType.CHAIN => false, // Add check
            SuperType.TIME => false, // Add check
            SuperType.TAUNT => StateMachine.IsNeutral(),
            _ => false,
        };
    }

    private void UpdateAura()
    {
        var curveDirection = m_Aura.activeSelf ? 1f : -1f;

        m_AuraCurveTime += curveDirection * Time.deltaTime;
        m_AuraCurveTime = Mathf.Clamp(m_AuraCurveTime, 0f, m_AuraCurveDuration);

        var time = m_AuraCurveTime / m_AuraCurveDuration;
        var curve = m_AuraCurve.Evaluate(time);

        m_Aura.transform.localScale = Vector3.Lerp(Vector3.zero, m_OriginalAuraSize, curve);

        if (m_AuraCurveTime <= 0f) m_Aura.SetActive(false);
    }

    private void ActivateTaunt()
    {
        m_Aura.SetActive(true);
        m_AuraCurveTime = 0;
    }

    public SuperData GetSuperData()
    {
        for (int i = 0; i < m_AllSupers.Length; i++)
        {
            if (m_AllSupers[i].superType == m_CurrentSuper) return m_AllSupers[i];
        }
        return null;
    }
}
