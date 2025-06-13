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
    public OathType SealType;

    public StatModifier(string sourceId, StatType statType, OperatorType op, float value, OathType sealType = OathType.None)
    {
        SourceId = sourceId;
        StatType = statType;
        Operator = op;
        Value = value;

        if (statType == StatType.SealModifier && sealType == OathType.None)
            Debug.LogError("Stat Modifier for SealType Resistance doesnt have an Seal.");
        else
            SealType = sealType;
    }

    public void Modify(StatQuery query)
    {
        if (query.Stat == StatType.SealModifier && SealType == OathType.None && SealType != query.SealType)
        {
            return; //Modifier Seal doesnt match Query.
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