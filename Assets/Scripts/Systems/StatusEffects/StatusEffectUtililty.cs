public static class StatusEffectUtility
{
    public static bool IsRootEffect(StatusEffectType type)
    {
        return type == StatusEffectType.Webbed ||
                type == StatusEffectType.Trapped ||
                type == StatusEffectType.Rooted;
    }
    public static bool IsStunEffect(StatusEffectType type)
    {
        return type == StatusEffectType.Frozen ||
                type == StatusEffectType.Stunned;
    }
}