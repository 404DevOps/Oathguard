using System;
using UnityEngine;

[Serializable]
public class StatModifier
{
    [HideInInspector]
    public string SourceId;
    public StatType StatType;
    public OperatorType Operator;
    public float Value;
    public OathType OathType;

    public StatModifier(string sourceId, StatType statType, OperatorType op, float value, OathType oathType = OathType.None)
    {
        SourceId = sourceId;
        StatType = statType;
        Operator = op;
        Value = value;

        if (statType == StatType.OathModifier && oathType == OathType.None)
            Debug.LogError("Stat Modifier for OathType Resistance doesnt have an Oath.");
        else
            OathType = oathType;
    }

    public void Modify(StatQuery query)
    {
        if (query.Stat == StatType.OathModifier && OathType == OathType.None && OathType != query.OathType)
        {
            return; //Modifier Oath doesnt match Query.
        }
        switch (Operator)
        {
            case OperatorType.AddPercentage:
                query.Value += (query.Value / 100) * Value;
                break;
            case OperatorType.SubtractPercentage:
                query.Value -= (query.Value / 100) * Value;
                break;
            case OperatorType.Multiply:
                query.Value = query.Value * Value;
                break;
            case OperatorType.Divide:
                query.Value = query.Value / Value;
                break;
            case OperatorType.Add:
                query.Value += Value;
                break;
            default:
                break;
        }
    }
}