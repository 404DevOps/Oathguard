public class StatInfo
{
    public StatInfo(StatType stat, float value, OathType oath = OathType.None)
    {
        StatType = stat;
        Value = value;
        Oath = oath;
    }
    public StatType StatType;
    public OathType Oath;
    public float Value;
}