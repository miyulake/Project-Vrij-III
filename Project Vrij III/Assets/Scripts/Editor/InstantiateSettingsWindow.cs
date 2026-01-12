using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class InstantiateSettingsWindow : EditorWindow
{
    private ObjectField m_ObjectField;
    private Vector3Field m_PositionField;
    private Vector3Field m_RotationField;

    [MenuItem("MIYU/Instantiate Settings", priority = 1)]
    public static void ShowWindow() => GetWindow<InstantiateSettingsWindow>("Instantiate Settings");

    private void CreateGUI()
    {
        var root = rootVisualElement;
        root.Clear();

        var style = EditorAssets.GetStyleSheet();
        if (style != null) root.styleSheets.Add(style);

        EditorHelpers.SetupRootLayout(root);
        root.Add(EditorHelpers.CreateLabel("Settings for the <b>Spawn Object</b> button"));
        root.Add(CreateSettingsSection());
    }

    private VisualElement CreateSettingsSection()
    {
        var foldout = new Foldout
        {
            text = "Settings",
            value = false
        };
        foldout.AddToClassList("foldout-box");

        // Object
        m_ObjectField = EditorHelpers.CreateObjectField("Object to Spawn", InstantiateSettings.ObjectToSpawn, 
            newValue => InstantiateSettings.ObjectToSpawn = newValue);
        foldout.Add(m_ObjectField);

        // Position
        m_PositionField = EditorHelpers.CreateVector3Field("Spawn Position", InstantiateSettings.SpawnPosition,
            newValue => InstantiateSettings.SpawnPosition = newValue);
        foldout.Add(m_PositionField);
        foldout.Add(EditorHelpers.CreateButton("Randomize Position", RandomizePosition, MiyuTooltips.TipRandomPos));

        // Rotation
        m_RotationField = EditorHelpers.CreateVector3Field("Spawn Rotation", InstantiateSettings.SpawnRotation,
            newValue => InstantiateSettings.SpawnRotation = newValue);
        foldout.Add(m_RotationField);
        foldout.Add(EditorHelpers.CreateButton("Randomize Rotation", RandomizeRotation, MiyuTooltips.TipRandomRot));

        // Reset
        foldout.Add(EditorHelpers.CreateButton("Reset", ResetWindow, MiyuTooltips.TipWindowReset));

        return foldout;
    }

    #region Helpers
    private void AssignObjectField(ObjectField field, System.Func<GameObject> generator, System.Action<GameObject> assign)
    {
        var value = generator();
        assign(value);
        field.SetValueWithoutNotify(value);
    }

    private void RandomizeVector3Field(Vector3Field field, System.Func<Vector3> generator, System.Action<Vector3> assign)
    {
        var value = generator();
        assign(value);
        field.SetValueWithoutNotify(value);
    }

    private void RandomizePosition()
    {
        var randomX = Random.Range(-10f, 10f);
        var randomY = Random.Range(-10f, 10f);
        var randomZ = Random.Range(-10f, 10f);
        RandomizeVector3Field(m_PositionField, () => new (randomX, randomY, randomZ
            ), value => InstantiateSettings.SpawnPosition = value);
    }

    private void RandomizeRotation()
    {
        var randomX = Random.Range(0, 360f);
        var randomY = Random.Range(0, 360f);
        var randomZ = Random.Range(0, 360f);
        RandomizeVector3Field(m_RotationField, () => new (randomX, randomY, randomZ
            ), value => InstantiateSettings.SpawnRotation = value);
    }

    private void ResetWindow()
    {
        AssignObjectField(m_ObjectField, null, value => InstantiateSettings.ObjectToSpawn = value);
        RandomizeVector3Field(m_PositionField, () => Vector3.zero, value => InstantiateSettings.SpawnPosition = value);
        RandomizeVector3Field(m_RotationField, () => Vector3.zero, value => InstantiateSettings.SpawnRotation = value);
    }
    #endregion
}

public struct InstantiateSettings
{
    public static GameObject ObjectToSpawn;
    public static Vector3 SpawnPosition;
    public static bool RandomizePosition;
    public static Vector3 SpawnRotation;
    public static bool RandomizeRotation;
}