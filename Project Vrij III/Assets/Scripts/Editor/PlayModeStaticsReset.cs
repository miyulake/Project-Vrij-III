#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public static class PlayModeStaticsReset
{
    static PlayModeStaticsReset() => EditorApplication.playModeStateChanged += OnPlayModeChanged;

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.ExitingEditMode || !EditorHelpers.IsDomainReloadDisabled()) return;
        StaticUtils.ResetAll();
    }
}
#endif