public class StatInfo
{
    public StatInfo(StatType stat, float value, SealType seal = SealType.None)
    {
        StatType = stat;
        Value = value;
        Seal = seal;
    }
    public StatType StatType;
    public SealType Seal;
    public float Value;
}