using Game.Entities;
using UnityEngine;

public class SuperHandler : EntityComponent, ITickable
{
    [SerializeField] private GameObject m_Aura;
    [SerializeField] private AnimationCurve m_AuraCurve;
    [SerializeField] private float m_AuraCurveDuration = 1f;
    private SuperData[] m_AllSupers;
    private Vector3 m_OriginalAuraSize;
    private float m_AuraCurveTime = 0;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        m_AllSupers = Entity.Character.AllSupers;
        m_OriginalAuraSize = m_Aura.transform.localScale;
        m_Aura.transform.localScale = Vector3.zero;
    }

    public void Tick()
    {
        /*
        if (RoundManager.Instance.CurrentState != RoundState.GAMEPLAY) return;

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1)) ResourcesComp.AddMeter(GameManager.Instance.GetMaxMeter());
        #endif

        UpdateAura();
        if (InputComp.Super)
        {
            Debug.Log($"Meter before super: {ResourcesComp.Meter.Current}");
            for (int i = 0; i < m_AllSupers.Length; i++)
            {
                if (m_AllSupers[i].CanActivate(Entity))
                {
                    ExecuteSuper(m_AllSupers[i]);
                    Debug.Log($"Meter after super: {ResourcesComp.Meter.Current}");
                    break;
                }
            }
        }
        */
    }

    private void ExecuteSuper(SuperData data)
    {
        // TO-DO: Check for specific scenarios and check if the super can be performed within those scenarios.
        switch (data.superType)
        {
            case SuperType.CHAIN: ActivateChain(); break;
            case SuperType.TIME:  ActivateTime();  break;
            case SuperType.TAUNT: ActivateTaunt(); break;
        }
        AttackComp.StartMove(data);
        StateMachine.ChangeState<SuperState>(false, data.freezeFrames, data.activationFrames);
        ResourcesComp.Meter.Modify(-data.meterCost);
        Debug.Log($"Meter after super: {ResourcesComp.Meter.Current}");
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

    private void ActivateChain()
    {
        print("Grab Super activated!");
    }

    private void ActivateTaunt()
    {
        m_Aura.SetActive(true);
        m_AuraCurveTime = 0;
    }

    private void ActivateTime()
    {
        //
    }
}
