public struct ResourceChangedEventArgs
{
    public EntityBase Entity;
    public float CurrentResource;
    public float MaxResource;

    public ResourceChangedEventArgs(EntityBase entityBase, float currentResource, float maxResource)
    {
        Entity = entityBase;
        CurrentResource = currentResource;
        MaxResource = maxResource;
    }
}