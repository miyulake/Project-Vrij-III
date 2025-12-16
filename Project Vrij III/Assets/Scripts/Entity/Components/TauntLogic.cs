using UnityEngine;

public class TauntLogic : EntityComponent
{
    public bool HasCompletedTaunt { get; private set; }
    [SerializeField] private GameObject m_EyeLights;
    [SerializeField] private AudioClip m_TauntActive;
    [SerializeField] private AudioClip m_TauntDeactive;
    [SerializeField] private float m_TauntMultiplier = 1.5f;
    [SerializeField] private int m_TauntIncrease = 2;

    public void ActivateTaunt()
    {
        var usingPaint = GameManager.Instance.CurrentMode == GameMode.PAINT;
        if (usingPaint) return;

        HasCompletedTaunt = !HasCompletedTaunt;

        m_EyeLights.SetActive(HasCompletedTaunt);

        if (HasCompletedTaunt) Entity.Audio.Play(m_TauntActive);
        else Entity.Audio.Play(m_TauntDeactive);
    }

    // Bad - should add to a global mutliplier
    public float GetMultiplier() =>
        HasCompletedTaunt ? m_TauntMultiplier : 1f;

    // Bad - should add to a global flat increase
    public int GetFlatIncrease() =>
        HasCompletedTaunt ? m_TauntIncrease : 0;

    public void Reset()
    {
        HasCompletedTaunt = false;
        m_EyeLights.SetActive(false);
    }
}
