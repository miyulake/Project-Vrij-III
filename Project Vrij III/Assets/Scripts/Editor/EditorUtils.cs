using UnityEditor;

public static class EditorUtils
{
    public static bool IsDomainReloadDisabled() =>
        EditorSettings.enterPlayModeOptionsEnabled &&
        EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload);
}
