using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EntityDefinition", menuName = "EntitySO/Definition")]
public class EntityDefinition : ScriptableObject { public List<SerializableType> components; }

[System.Serializable]
public class SerializableType
{
    public string assemblyQualifiedName;
    public Type GetCompType() => Type.GetType(assemblyQualifiedName);
}
