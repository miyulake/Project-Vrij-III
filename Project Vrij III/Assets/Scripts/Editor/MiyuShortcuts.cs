using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine.SceneManagement;

public class MiyuShortcuts : EditorWindow
{
    [MenuItem("MIYU/Tool Window/Compile Scripts")]
    private static void CompileShortcut() => CompilationPipeline.RequestScriptCompilation();

    [MenuItem("MIYU/Tool Window/Reset Statics")]
    private static void ResetShortcut() => StaticUtils.ResetAll();

    [MenuItem("MIYU/Tool Window/Reload Scene")]
    private static void ReloadShortcut() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    [MenuItem("MIYU/Tool Window/Spawn Object")]
    private static void SpawnObjectShortcut() { }
}
