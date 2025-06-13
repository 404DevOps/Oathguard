public class StatInfo
{
    public StatInfo(StatType stat, float value, OathType seal = OathType.None)
    {
        StatType = stat;
        Value = value;
        Seal = seal;
    }
    public StatType StatType;
    public OathType Seal;
    public float Value;
}