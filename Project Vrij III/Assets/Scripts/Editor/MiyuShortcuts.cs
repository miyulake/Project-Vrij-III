using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine.SceneManagement;

public class MiyuShortcuts : EditorWindow
{
    [MenuItem("MIYU/Shortcuts/Compile Scripts")]
    private static void CompileShortcut() => CompilationPipeline.RequestScriptCompilation();

    [MenuItem("MIYU/Shortcuts/Reset Statics")]
    private static void ResetShortcut() => StaticUtils.ResetAll();

    [MenuItem("MIYU/Shortcuts/Reload Scene")]
    private static void ReloadShortcut() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    [MenuItem("MIYU/Shortcuts/Spawn Object")]
    private static void SpawnObjectShortcut() { }
}
