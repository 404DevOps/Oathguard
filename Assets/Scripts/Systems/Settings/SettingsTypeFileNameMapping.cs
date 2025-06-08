using System;
using UnityEngine;

[Serializable]
public class SettingsTypeFileNameMapping
{
    public string FileName;

    // Internally store the type as a string
    [SerializeField]
    private string typeName;

    // Expose the Type as a property
    public Type Type
    {
        get => Type.GetType(typeName);
        set => typeName = value?.AssemblyQualifiedName;
    }
}