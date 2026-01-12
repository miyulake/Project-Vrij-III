using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public static class EditorAssets
{
    public static GUIContent GetWindowIcon()
    {
        var path = AssetDatabase.GUIDToAssetPath("8392c84c83feeb14e92305e81321982a");
        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        return new("\u2003MIYU Tools", icon); // \u2003 adds spacing
    }

    public static StyleSheet GetStyleSheet()
    {
        var path = AssetDatabase.GUIDToAssetPath("1daf168968711714ca3be0f6cb8c9e0a");
        return AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
    }

    public static AudioClip GetButtonSound(string name)
    {
        var path = "";

        if (name == "Meow")
            path = AssetDatabase.GUIDToAssetPath("890ed5dcd617e0f42b6d42d652745f01");
        else if (name == "Meow_Long")
            path = AssetDatabase.GUIDToAssetPath("0b39c32df0951d648b81833c233dde9f");

        return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    }

    public static GameObject GetTestObject()
    {
        /*
        var path = AssetDatabase.GUIDToAssetPath("678267692256ab04792d03705ceaa704");
        return AssetDatabase.LoadAssetAtPath<GameObject>(path);
        '*/
        return null;
    }
}
