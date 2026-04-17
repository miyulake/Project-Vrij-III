// Source - https://stackoverflow.com/a/68867019
// Posted by Ahnaf, modified by community. See post 'Timeline' for change history
// Retrieved 2026-04-03, License - CC BY-SA 4.0

using UnityEngine;
using UnityEditor;

namespace Miyu.Shaders.Properties
{
    /// <summary>
    /// Draws a vector2 field for vector properties.
    /// Usage: [ShowAsVector2] _Vector2("Vector 2", Vector) = (0,0,0,0)
    /// </summary>
    public class ShowAsVector2Drawer : MaterialPropertyDrawer
    {
        public override void OnGUI(Rect position, MaterialProperty property, GUIContent label, MaterialEditor editor)
        {
            if (property.propertyType == UnityEngine.Rendering.ShaderPropertyType.Vector)
            {
                EditorGUIUtility.labelWidth = 0f;
                EditorGUIUtility.fieldWidth = 0f;

                if (!EditorGUIUtility.wideMode)
                {
                    EditorGUIUtility.wideMode = true;
                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - 212;
                }

                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = property.hasMixedValue;

                var vector = EditorGUI.Vector2Field(position, label, property.vectorValue);
                if (EditorGUI.EndChangeCheck()) property.vectorValue = vector;
            }
            else editor.DefaultShaderProperty(property, label.text);
        }
    }
}