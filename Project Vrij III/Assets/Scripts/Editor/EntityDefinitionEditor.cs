using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(EntityDefinition))]
public class EntityDefinitionEditor : Editor
{
    private EntityDefinition m_Definition;
    private List<Type> m_ComponentTypes;

    private void OnEnable()
    {
        m_Definition = (EntityDefinition)target;
        m_ComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(Game.Entities.IEntityComponent).IsAssignableFrom(t) && 
            !t.IsAbstract && 
            !typeof(MonoBehaviour).IsAssignableFrom(t))
            .ToList();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.LabelField("Pure C# Components", EditorStyles.boldLabel);

        for (int i = 0; i < m_Definition.components.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            var currentIndex = 
                m_ComponentTypes.FindIndex(t => t.AssemblyQualifiedName == m_Definition.components[i].assemblyQualifiedName);
            if (currentIndex < 0) currentIndex = 0;

            var selectedIndex = EditorGUILayout.Popup(currentIndex, m_ComponentTypes.Select(t => t.Name).ToArray());
            m_Definition.components[i].assemblyQualifiedName = m_ComponentTypes[selectedIndex].AssemblyQualifiedName;

            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                m_Definition.components.RemoveAt(i);
                i--;
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Component"))
            m_Definition.components.Add
                (new SerializableType { assemblyQualifiedName = m_ComponentTypes[0].AssemblyQualifiedName });

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(m_Definition);
    }
}
