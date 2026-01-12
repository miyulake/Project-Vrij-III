using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;

#if UNITY_EDITOR
public static class EditorHelpers
{
    // Keep a state button list if needed
    public static List<(Button button, Func<bool> activeState)> StateButtons = new();

    /// <summary>
    /// Sets up the base root layout for the window.
    /// </summary>
    public static void SetupRootLayout(VisualElement root)
    {
        root.style.paddingLeft = 10;
        root.style.paddingRight = 10;
        root.style.paddingTop = 10;
        root.style.flexDirection = FlexDirection.Column;
        root.Add(CreateLabel("© 2026 Jesse Westerlaken", "copyright"));
    }

    /// <summary>
    /// Create a normal button with optional active state.
    /// </summary>
    public static Button CreateButton(string text, Action onClick, string tooltip = "", 
        Func<bool> activeState = null, List<(Button button, Func<bool> activeState)> targetList = null)
    {
        var clickSound = EditorAssets.GetButtonSound("Meow");
        var button = new Button
        {
            text = text,
            tooltip = tooltip
        };

        button.clicked += () =>
        {
            EditorAudio.PlayClip(clickSound, true);
            onClick?.Invoke();

            if (activeState != null) button.SetEnabled(activeState());
        };

        if (activeState != null)
        {
            StateButtons.Add((button, activeState));
            targetList?.Add((button, activeState));
            button.SetEnabled(activeState());
        }

        button.AddToClassList("button");
        return button;
    }

    /// <summary>
    /// Create a secret button for fun.
    /// </summary>
    public static Button CreateSecretButton(string text)
    {
        var clickSound = EditorAssets.GetButtonSound("Meow_Long");
        var message =
            "meow meow meow meow meow meow meow meow meow meow meow meow meow meow meow meow" +
            " meow meow meow meow meow meow meow meow meow meow meow meow meow meow meow meow";

        var button = new Button(() =>
        {
            EditorAudio.PlayClip(clickSound, true);
            EditorUtility.DisplayDialog(":3", message, "Miaow");
        }) { text = text };

        button.AddToClassList("secret-button");
        return button;
    }

    /// <summary>
    /// Create a Label with optional USS class.
    /// </summary>
    public static Label CreateLabel(string text, string style = "label")
    {
        var label = new Label { text = text };
        label.AddToClassList(style);
        return label;
    }

    public static ObjectField CreateObjectField<T>(string label, T value, Action<T> onChange = null) where T : UnityEngine.Object
    {
        var field = new ObjectField(label)
        {
            objectType = typeof(T),
            allowSceneObjects = true,
            value = value
        };
        if (onChange != null) field.RegisterValueChangedCallback(evt => onChange((T)evt.newValue));
        field.AddToClassList("object-field");
        return field;
    }

    public static Vector3Field CreateVector3Field(string label, Vector3 value, Action<Vector3> onChange)
    {
        var field = new Vector3Field { label = label, value = value };
        if (onChange != null) field.RegisterValueChangedCallback(@event => onChange(@event.newValue));
        field.AddToClassList("vector-3-field");
        return field;
    }

    public static void RecordUndo(UnityEngine.Object @object,string name)
    {
        Undo.RecordObject(@object, name);
        EditorUtility.SetDirty(@object);
    }

    /// <summary>
    /// Domain reload check helper.
    /// </summary>
    public static bool IsDomainReloadDisabled() =>
        EditorSettings.enterPlayModeOptionsEnabled &&
        EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload);
}
#endif
