public struct HealEventArgs
{
    public HealEventArgs(EntityBase origin, EntityBase target, float amount)
    { 
        Origin = origin;
        Target = target;
        Amount = amount;
    }
    public EntityBase Origin;
    public EntityBase Target;
    public float Amount;
}