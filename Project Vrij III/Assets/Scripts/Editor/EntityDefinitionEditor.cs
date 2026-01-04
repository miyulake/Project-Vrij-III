using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(EntityDefinition))]
public class EntityDefinitionEditor : Editor
{
    private EntityDefinition m_Definition;
    private List<Type> m_ComponentTypes;
    private ReorderableList m_ReorderableList;

    private void OnEnable()
    {
        m_Definition = (EntityDefinition)target;

        var assembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");

        if (assembly != null)
        {
            m_ComponentTypes = assembly.GetTypes()
                .Where(t => typeof(Game.Entities.IEntityComponent).IsAssignableFrom(t)
                            && !t.IsAbstract
                            && !typeof(MonoBehaviour).IsAssignableFrom(t))
                .ToList();
        }

        m_ReorderableList = new ReorderableList(
            m_Definition.components, 
            typeof(SerializableType), 
            draggable: true, 
            displayHeader: true, 
            displayAddButton: true, 
            displayRemoveButton: true)
        {
            drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, "Pure C# Components");
            },

            drawElementCallback = (rect, index, active, focused) =>
            {
                rect.y += 2;

                var element = m_Definition.components[index];
                var currentIndex = m_ComponentTypes.FindIndex(
                    t => t.AssemblyQualifiedName == element.assemblyQualifiedName);

                if (currentIndex < 0) currentIndex = 0;

                var selectedIndex = EditorGUI.Popup(
                    rect,
                    currentIndex,
                    m_ComponentTypes.Select(t => t.Name).ToArray()
                );

                element.assemblyQualifiedName = m_ComponentTypes[selectedIndex].AssemblyQualifiedName;
            }
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        m_ReorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(m_Definition);
    }
}
