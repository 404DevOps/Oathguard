using System.Collections.Generic;
using UnityEngine;

public class ImmunityHandler : MonoBehaviour
{
    private readonly Dictionary<DamageType, HashSet<string>> _immunitySources = new();

    public bool IsImmuneTo(DamageType type)
    {
        return _immunitySources.TryGetValue(type, out var sources) && sources.Count > 0;
    }

    public void AddImmunity(DamageType type, string sourceId)
    {
        if (!_immunitySources.TryGetValue(type, out var sources))
            _immunitySources[type] = sources = new HashSet<string>();

        sources.Add(sourceId);
    }

    public void RemoveImmunity(DamageType type, string sourceId)
    {
        if (_immunitySources.TryGetValue(type, out var sources))
        {
            sources.Remove(sourceId);
            if (sources.Count == 0)
                _immunitySources.Remove(type);
        }
    }
}