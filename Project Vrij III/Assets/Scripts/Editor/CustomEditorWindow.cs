#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;

public class CustomEditorWindow : EditorWindow
{
    // Play Mode Buttons
    private VisualElement m_PlayModeContainer;
    private Label m_DomainLabel;

    private readonly List<(Button button, System.Func<bool> activeState)> m_StateButtons =
        new();

    [MenuItem("MIYU/Tool Window", priority = 0)]
    public static void ShowWindow()
    {
        var window = GetWindow<CustomEditorWindow>("MIYU Tools");
        window.titleContent = EditorAssets.GetWindowIcon();
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
        m_StateButtons.Clear(); // Clear states in this instance
    }

    private void OnDestroy() => EditorAudio.StopAllClips();

    private void CreateGUI()
    {
        var root = rootVisualElement;
        root.Clear();

        var style = EditorAssets.GetStyleSheet();
        if (style != null) root.styleSheets.Add(style);

        EditorHelpers.SetupRootLayout(root);
        root.Add(EditorHelpers.CreateLabel("General development helper tools"));
        root.Add(CreateCoreSection());
        root.Add(CreatePlayModeSection());
        root.Add(EditorHelpers.CreateSecretButton("<b>♡ (˶˃ ᵕ ˂˶) ♡"));

        UpdatePlayModeState();
    }

    private void EditorTick()
    {
        if (m_DomainLabel != null) m_DomainLabel.text = GetDomainText();
    }
    #endregion

    #region Sections
    private VisualElement CreateCoreSection()
    {
        var foldout = new Foldout
        {
            text = "Core",
            value = false
        };
        foldout.AddToClassList("foldout-box");

        m_DomainLabel = EditorHelpers.CreateLabel(GetDomainText());
        foldout.Add(m_DomainLabel);

        foldout.Add(EditorHelpers.CreateButton("Compile Scripts", 
            CompilationPipeline.RequestScriptCompilation, MiyuTooltips.FormatDanger + MiyuTooltips.TipCompile));
        foldout.Add(EditorHelpers.CreateButton("Reset Statics", 
            StaticUtils.ResetAll, MiyuTooltips.FormatWarning + MiyuTooltips.TipStatics));

        return foldout;
    }

    private VisualElement CreatePlayModeSection()
    {
        var foldout = new Foldout
        {
            text = "Play Mode",
            value = false
        };
        foldout.AddToClassList("foldout-box");
        foldout.Add(EditorHelpers.CreateLabel("Enter Play Mode to enable buttons"));

        m_PlayModeContainer = new ();
        m_PlayModeContainer.AddToClassList("playmode-container");

        m_PlayModeContainer.Add(EditorHelpers.CreateButton("Reload Scene", 
            ReloadScene, MiyuTooltips.TipScene));
        m_PlayModeContainer.Add(EditorHelpers.CreateButton("Spawn Object",
            SpawnObject, MiyuTooltips.TipSpawn));

        foldout.Add(m_PlayModeContainer);

        return foldout;
    }
    #endregion

    #region Helpers
    private string GetDomainText()
    {
        var disabled = EditorHelpers.IsDomainReloadDisabled();
        return
            $"Domain reload disabled: <b><color={(disabled ? "green" : "red")}>{disabled}</color></b>\n" +
            "(Reset Statics auto triggers on play)";
    }

    private void OnPlayModeChanged(PlayModeStateChange _)
    {
        UpdateButtonStates();
        UpdatePlayModeState();
    }

    private void UpdatePlayModeState()
    {
        if (m_PlayModeContainer != null) m_PlayModeContainer.SetEnabled(EditorApplication.isPlaying);
    }

    private void UpdateButtonStates()
    {
        foreach (var (button, predicate) in m_StateButtons) button.SetEnabled(predicate());
    }
    #endregion

    #region Functions
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        UpdateButtonStates();
    }

    private void SpawnObject()
    {
        if (InstantiateSettings.ObjectToSpawn == null)
        {
            Debug.LogWarning("No object selected to spawn.");
            return;
        }

        GameObject @object;
        if (PrefabUtility.IsPartOfPrefabAsset(InstantiateSettings.ObjectToSpawn))
            @object = (GameObject)PrefabUtility.InstantiatePrefab(InstantiateSettings.ObjectToSpawn);
        else
        {
            @object = Instantiate(InstantiateSettings.ObjectToSpawn);
            @object.name = InstantiateSettings.ObjectToSpawn.name;
        }
        @object.transform.SetPositionAndRotation(InstantiateSettings.SpawnPosition, Quaternion.Euler(InstantiateSettings.SpawnRotation));

        Undo.RegisterCreatedObjectUndo(@object, "Spawn Object");
        Selection.activeGameObject = @object;
    }
    #endregion
}
#endif