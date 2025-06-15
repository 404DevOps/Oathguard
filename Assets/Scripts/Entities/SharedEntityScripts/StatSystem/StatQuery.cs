using System;

public class StatQuery
{
    public StatQuery(StatType stat, float baseValue, OathType? oathtype = null)
    {
        Stat = stat;
        BaseValue = baseValue;
        Value = baseValue;
        OathType = oathtype;
    }

    public StatType Stat;
    public float BaseValue;
    public float Value;
    public OathType? OathType;
}