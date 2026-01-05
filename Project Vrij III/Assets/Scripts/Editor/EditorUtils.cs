#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

public class EditorUtils : MonoBehaviour
{
    [MenuItem("Tools/MIYU/Reload Domain %#d")] // Ctrl+Shift+D
    public static void ReloadDomain() => CompilationPipeline.RequestScriptCompilation();

    [MenuItem("Tools/MIYU/Reset Statics %#r")] // Ctrl+Shift+R
    public static void ResetStatics() => StaticUtils.ResetAll();

    public static bool IsDomainReloadDisabled() =>
        EditorSettings.enterPlayModeOptionsEnabled &&
        EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload);
}
#endif