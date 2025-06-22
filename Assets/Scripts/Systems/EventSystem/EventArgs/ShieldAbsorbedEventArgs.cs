public struct ShieldAbsorbedEventArgs
{
    public ShieldAbsorbedEventArgs(EntityBase entity, float amount)
    {
        Entity = entity;
        AbsorbedAmount = amount;
    }
    public EntityBase Entity;
    public float AbsorbedAmount;
}