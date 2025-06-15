using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class CoroutineUtility : Singleton<CoroutineUtility>
{
    List<TrackedCoroutine> _generalCoroutines = new List<TrackedCoroutine>();
    Dictionary<string, List<TrackedCoroutine>> _abilityCoroutines = new();

    public Coroutine RunAbilityCoroutine(IEnumerator action, string abilityId)
    {
        TrackedCoroutine tracked = new();
        tracked.Enumerator = action;
        tracked.CoroutineReference = StartCoroutine(WrapAbilityCoroutine(tracked, abilityId));

        if (!_abilityCoroutines.ContainsKey(abilityId))
            _abilityCoroutines[abilityId] = new List<TrackedCoroutine>();

        _abilityCoroutines[abilityId].Add(tracked);
        return tracked.CoroutineReference;
    }
    private IEnumerator WrapAbilityCoroutine(TrackedCoroutine tracked, string abilityId)
    {
        yield return tracked.Enumerator;

        if (_abilityCoroutines.TryGetValue(abilityId, out var list))
        {
            list.Remove(tracked);
            if (list.Count == 0)
                _abilityCoroutines.Remove(abilityId);
        }
    }
    private IEnumerator WrapGeneralCoroutine(TrackedCoroutine tracked)
    {
        yield return tracked.Enumerator;
        _generalCoroutines.Remove(tracked);
    }
    public Coroutine RunCoroutineTracked(IEnumerator action)
    {
        TrackedCoroutine tracked = new TrackedCoroutine();
        tracked.Enumerator = action;
        tracked.CoroutineReference = StartCoroutine(WrapGeneralCoroutine(tracked));
        _generalCoroutines.Add(tracked);
        return tracked.CoroutineReference;
    }
    public void AbortAllCoroutines()
    {
        foreach (var tracked in _generalCoroutines)
        {
            if (tracked != null && tracked.CoroutineReference != null)
                StopCoroutine(tracked.CoroutineReference);
        }
        _generalCoroutines.Clear();

        foreach (var kvp in _abilityCoroutines)
        {
            AbortAllAbilityCoroutines(kvp.Key, false);
        }
        _abilityCoroutines.Clear();
    }
    public void AbortAllAbilityCoroutines(string abilityId, bool removeInstant = true)
    {
        if (_abilityCoroutines.TryGetValue(abilityId, out var list))
        {
            foreach (var coroutine in list)
            {
                if (coroutine != null && coroutine.CoroutineReference != null)
                    StopCoroutine(coroutine.CoroutineReference);
            }
            if (removeInstant)
                _abilityCoroutines.Remove(abilityId);
        }
    }
}

internal class TrackedCoroutine
{
    public Coroutine CoroutineReference;
    public IEnumerator Enumerator;
}