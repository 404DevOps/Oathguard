public struct ShieldChangedEventArgs
{
    public EntityBase Entity;
    public float CurrentShield;

    public ShieldChangedEventArgs(EntityBase entityBase, float currentShield)
    {
        Entity = entityBase;
        CurrentShield = currentShield;
    }
}