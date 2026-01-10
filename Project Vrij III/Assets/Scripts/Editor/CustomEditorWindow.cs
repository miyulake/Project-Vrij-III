#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CustomEditorWindow : EditorWindow
{
    private Button m_ReloadButton;
    private Label m_DomainLabel;

    [MenuItem("MIYU/Tools")]
    public static void ShowWindow()
    {
        var window = GetWindow<CustomEditorWindow>("MIYU Tools");
        window.SetWindowIcon();
    }

    #region Setup
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        EditorApplication.update += EditorTick;
        //EditorAudio.PlayClip(GetButtonSound("Meow_Long"));
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorApplication.update -= EditorTick;
    }

    private void OnDestroy() => EditorAudio.StopAllClips();

    public void CreateGUI()
    {
        var root = rootVisualElement;
        root.Clear();

        var style = GetStyleSheet();
        if (style != null) root.styleSheets.Add(style);

        SetupRootLayout(root);
        root.Add(CreateSecretButton("<b>♡ (˶˃ ᵕ ˂˶) ♡"));
        root.Add(CreateCoreSection());
        root.Add(CreatePlayModeSection());

        UpdatePlayModeState();
    }

    private void EditorTick()
    {
        if (m_DomainLabel != null) m_DomainLabel.text = GetDomainText();
    }

    private void SetupRootLayout(VisualElement root)
    {
        root.style.paddingLeft = 10;
        root.style.paddingRight = 10;
        root.style.paddingTop = 10;
        root.style.flexDirection = FlexDirection.Column;
    }
    #endregion

    #region Sections
    private VisualElement CreateCoreSection()
    {
        var foldout = new Foldout
        {
            text = "Core",
            value = true
        };
        foldout.AddToClassList("foldout-box");

        m_DomainLabel = CreateLabel(GetDomainText());
        foldout.Add(m_DomainLabel);
        foldout.Add(CreateButton("Compile Scripts", 
            CompilationPipeline.RequestScriptCompilation, MiyuTooltips.FormatDanger + MiyuTooltips.TipCompile));
        foldout.Add(CreateButton("Reset Statics", 
            StaticUtils.ResetAll, MiyuTooltips.FormatWarning + MiyuTooltips.TipStatics));

        return foldout;
    }

    private VisualElement CreatePlayModeSection()
    {
        var foldout = new Foldout
        {
            text = "Play Mode",
            value = true
        };
        foldout.AddToClassList("foldout-box");

        foldout.Add(CreateLabel("Enter Play Mode to enable buttons"));
        m_ReloadButton = CreateButton("Reload Scene", ReloadScene, MiyuTooltips.TipScene);
        foldout.Add(m_ReloadButton);

        return foldout;
    }
    #endregion

    #region Helpers
    private Button CreateButton(string text, System.Action onClick, string tooltip = "")
    {
        var button = new Button(() =>
        {
            EditorAudio.PlayClip(GetButtonSound("Meow"), true);
            onClick?.Invoke();
        }){ text = text, tooltip = tooltip };

        button.AddToClassList("button");
        return button;
    }

    private Button CreateSecretButton(string text)
    {
        var message = "meow meow meow meow meow meow meow meow meow meow meow meow meow meow meow meow";
        var button = new Button(() =>
        {
            EditorAudio.PlayClip(GetButtonSound("Meow_Long"), true);
            EditorUtility.DisplayDialog(":3", message, "Miaow");
        }){ text = text };

        button.AddToClassList("secret-button");
        return button;
    }

    private Label CreateLabel(string text)
    {
        var label = new Label{ text = text };
        label.AddToClassList("label");
        return label;
    }

    private string GetDomainText()
    {
        var disabled = EditorUtils.IsDomainReloadDisabled();
        return
            $"Domain reload disabled: <b><color={(disabled ? "green" : "red")}>{disabled}</color></b>\n" +
            "(Reset Statics auto triggers on play)";
    }

    private void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    private void OnPlayModeChanged(PlayModeStateChange _) => UpdatePlayModeState();

    private void UpdatePlayModeState()
    {
        if (m_ReloadButton != null) m_ReloadButton.SetEnabled(EditorApplication.isPlaying);
    }

    private void SetWindowIcon()
    {
        var path = AssetDatabase.GUIDToAssetPath("8392c84c83feeb14e92305e81321982a");
        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (icon != null) titleContent = new GUIContent("\u2003MIYU Tools", icon); // \u2003 adds spacing
    }

    private StyleSheet GetStyleSheet()
    {
        var path = AssetDatabase.GUIDToAssetPath("1daf168968711714ca3be0f6cb8c9e0a");
        return AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
    }

    private AudioClip GetButtonSound(string name)
    {
        string path = "";

        if (name == "Meow") 
            path = AssetDatabase.GUIDToAssetPath("890ed5dcd617e0f42b6d42d652745f01");
        else if (name == "Meow_Long") 
            path = AssetDatabase.GUIDToAssetPath("0b39c32df0951d648b81833c233dde9f");

        return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    } 
    #endregion
}
#endif