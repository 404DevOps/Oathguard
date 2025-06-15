public struct HealingData
{
    public HealingData(EntityBase origin, EntityBase target, float damageAmount)
    {
        Origin = origin;
        Target = target;
        Amount = damageAmount;
    }

    public EntityBase Origin;
    public EntityBase Target;
    public float Amount;
}