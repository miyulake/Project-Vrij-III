using UnityEngine;
using Miyu.Tools;

public class PortalMovement : MonoBehaviour
{
    [SerializeField] private float m_StartValueX, m_TargetValueX;
    [SerializeField] private float m_StartValueY, m_TargetValueY;
    [SerializeField] private AnimationCurve m_MoveCurve;
    [SerializeField] private float m_Duration = 1f;
    [SerializeField] private bool m_Loop = true;
    [SerializeField] private bool m_RandomizeStart = true;

    private Tween m_Tween;
    private const string PROPERTY_ID = "_PortalCenter";

    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        var block = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(block);
        var vector = block.GetVector(PROPERTY_ID);
        
        m_Tween = new(m_MoveCurve, m_Duration, m_Loop);
        m_Tween.OnUpdate += value =>
        {
            block.SetVector(PROPERTY_ID, new(
                Mathf.Lerp(m_StartValueX, m_TargetValueX, value), 
                Mathf.Lerp(m_StartValueY, m_TargetValueY, value), 0, 0));
            renderer.SetPropertyBlock(block);
        };
        m_Tween.Play();
        if (m_RandomizeStart) m_Tween.Randomize();
    }

    private void Update() => m_Tween.Tick(Time.deltaTime);
}
