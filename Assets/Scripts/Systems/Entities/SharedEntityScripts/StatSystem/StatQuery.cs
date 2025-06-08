using System;

public class StatQuery
{
    public StatQuery(StatType stat, float baseValue, SealType? sealtype = null)
    {
        Stat = stat;
        BaseValue = baseValue;
        Value = baseValue;
        SealType = sealtype;
    }

    public StatType Stat;
    public float BaseValue;
    public float Value;
    public SealType? SealType;
}