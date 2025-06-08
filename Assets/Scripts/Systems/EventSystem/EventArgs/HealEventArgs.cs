public struct HealEventArgs
{
    public HealEventArgs(EntityBase entity, float amount)
    { 
        Entity = entity;
        Amount = amount;
    }
    public EntityBase Entity;
    public float Amount;
}