using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(EntityDefinition))]
public class EntityDefinitionEditor : Editor
{
    private ReorderableList list;
    private SerializedProperty componentsProp;

    private static List<Type> allComponentTypes;
    private static string[] allTypeNames;
    private static string[] allTypeAQNs;

    private List<int>[] availableIndicesCache;

    private void OnEnable()
    {
        componentsProp = serializedObject.FindProperty("components");
        CacheComponentTypes();
        SetupReorderableList();
    }

    private static void CacheComponentTypes()
    {
        if (allComponentTypes != null) return;

        allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => {
                try { return a.GetTypes(); } catch { return Array.Empty<Type>(); }
            })
            .Where(t => typeof(Game.Entities.IEntityComponent).IsAssignableFrom(t)
                        && !t.IsAbstract && !typeof(MonoBehaviour).IsAssignableFrom(t))
            .OrderBy(t => t.Name)
            .ToList();

        allTypeNames = allComponentTypes.Select(t => t.Name).ToArray();
        allTypeAQNs = allComponentTypes.Select(t => t.AssemblyQualifiedName).ToArray();
    }

    private void SetupReorderableList()
    {
        var headerStyle = new GUIStyle()
        {
            richText = true,
            alignment = TextAnchor.MiddleCenter,
        };

        list = new(serializedObject, componentsProp, true, true, true, true)
        {
            drawHeaderCallback = rect =>
                EditorGUI.LabelField(rect, "<color=white><u><b>Non-Mono</b></u> Entity Components</color>", headerStyle),

            drawElementCallback = DrawElement,

            onAddCallback = _ => { componentsProp.arraySize++; availableIndicesCache = null; },
            onReorderCallback = _ => { availableIndicesCache = null; },
            onCanAddCallback = _ => componentsProp.arraySize < allComponentTypes.Count
        };
    }

    private void RebuildAvailableCache()
    {
        var count = componentsProp.arraySize;
        availableIndicesCache = new List<int>[count];

        var used = new HashSet<string>();
        for (int i = 0; i < count; i++)
        {
            var aqn = componentsProp.GetArrayElementAtIndex(i)
                .FindPropertyRelative("assemblyQualifiedName").stringValue;
            if (!string.IsNullOrEmpty(aqn)) used.Add(aqn);
        }

        for (int i = 0; i < count; i++)
        {
            var elementProp = componentsProp.GetArrayElementAtIndex(i);
            var currentAQN = elementProp.FindPropertyRelative("assemblyQualifiedName").stringValue;

            var listIndices = new List<int>();
            for (int j = 0; j < allComponentTypes.Count; j++)
            {
                var aqn = allTypeAQNs[j];
                if (!used.Contains(aqn) || aqn == currentAQN) listIndices.Add(j);
            }
            availableIndicesCache[i] = listIndices;
        }
    }

    private void DrawElement(Rect rect, int index, bool _, bool __)
    {
        rect.y += 2;

        if (availableIndicesCache == null || availableIndicesCache.Length != componentsProp.arraySize)
            RebuildAvailableCache();

        var elementProp = componentsProp.GetArrayElementAtIndex(index);
        var aqnProp = elementProp.FindPropertyRelative("assemblyQualifiedName");

        var available = availableIndicesCache[index];
        if (available.Count == 0) return;

        var current = available.FindIndex(i => allTypeAQNs[i] == aqnProp.stringValue);
        if (current < 0) current = 0;

        EditorGUI.BeginChangeCheck();
        var names = available.ConvertAll(i => allTypeNames[i]).ToArray();
        var selected = EditorGUI.Popup(rect, current, names);
        if (EditorGUI.EndChangeCheck())
        {
            aqnProp.stringValue = allTypeAQNs[available[selected]];
            availableIndicesCache = null;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        if (allComponentTypes.Count == 0) EditorGUILayout.HelpBox("No components found.", MessageType.Warning);
        else
        {
            if (availableIndicesCache == null || availableIndicesCache.Length != componentsProp.arraySize) 
                availableIndicesCache = null;

            list.DoLayoutList();

            if (componentsProp.arraySize >= allComponentTypes.Count)
                EditorGUILayout.HelpBox("All possible components have been added.", MessageType.Info);
        }
        serializedObject.ApplyModifiedProperties();
    }
}