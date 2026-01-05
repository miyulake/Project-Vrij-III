#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

public class CustomEditorWindow : EditorWindow
{
    private static Texture2D m_CustomIcon;

    private void OnEnable()
    {
        var path = AssetDatabase.GUIDToAssetPath("8392c84c83feeb14e92305e81321982a");
        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (icon == null) 
        {
            Debug.LogWarning("Custom icon not found at: " + path);
            return;
        }
        m_CustomIcon = icon;
    }

    [MenuItem("MIYU/Tools")]
    public static void ShowWindow()
    {
        var window = GetWindow<CustomEditorWindow>();
        window.titleContent = new GUIContent("\u2003MIYU Tools", m_CustomIcon); // \u2003 adds spacing
        window.Show();
    }


    private void OnGUI()
    {
        EditorGUILayout.Space();
        CenteredLabel("<b>♡ (˶˃ ᵕ ˂˶) ♡", TextAnchor.MiddleCenter, 16);
        EditorGUILayout.Space();

        CenteredButton("Compile Scripts", () => CompilationPipeline.RequestScriptCompilation());
        CenteredButton("Reset Statics", () => StaticUtils.ResetAll());

        EditorGUILayout.Space();
        var domainReloadDisabled = EditorUtils.IsDomainReloadDisabled();
        CenteredLabel(
            $"Domain reload disabled: <b>{domainReloadDisabled}</b>\n" +
            "<i>(Reset Statics auto triggers on play)</i>", 
            TextAnchor.MiddleLeft);
    }

    private void CenteredLabel(string text, TextAnchor textAnchor, int fontSize = 12)
    {
        var centerStyle = new GUIStyle(GUI.skin.label)
        {
            richText = true,
            alignment = textAnchor,
            fontSize = fontSize,
        };
        GUILayout.Label(text, centerStyle);
    }

    private void CenteredButton(string text, System.Action onClick, float width = 200, float height = 30)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(text, GUILayout.Width(width), GUILayout.Height(height))) onClick?.Invoke();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }
}
#endif
