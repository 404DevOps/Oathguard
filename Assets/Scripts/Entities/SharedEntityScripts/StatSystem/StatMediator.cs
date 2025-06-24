using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StatMediator
{
    [SerializeField] private List<StatModifier> _modifiers = new List<StatModifier>();

    public void AddModifier(StatModifier modifier)
    {

        _modifiers.Add(modifier);
        Debug.Log("Modifier added with SourceId " + modifier.SourceId + ". Total: " + _modifiers.Count);
    }

    public void RemoveModifiersBySourceId(string sourceId)
    {
        _modifiers.RemoveAll(m => m.SourceId == sourceId);
        Debug.Log("Modifier removed with SourceId " + sourceId + ". Total: " + _modifiers.Count);
    }
    public float GetModifiedStatAfterEquipment(StatType stat, float baseValue)
    {
        //var query = new StatQuery(stat, baseValue);
        //foreach (var mod in _modifiers.Where(m => m.ModSourceType == ModifierSourceType.Equipment && m.StatType == stat))
        //{
        //    mod.Modify(query);
        //}

        //return query.Value;
        return 0f;
    }
    public float GetModifiedStat(StatType stat, float baseValue, OathType? oath = null)
    {
        var query = new StatQuery(stat, baseValue, oath);
        var modifiersForStat = _modifiers.Where(sm => sm.StatType == stat);

        foreach (var mod in modifiersForStat)
        {
            mod.Modify(query);
        }

        return query.Value;
    }

    internal float GetModifiedStat(StatType experienceRate1, object experienceRate2)
    {
        throw new NotImplementedException();
    }
}