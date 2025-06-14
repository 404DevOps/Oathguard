
public struct HealthChangedEventArgs
{
    public HealthChangedEventArgs(EntityBase target, float currentHealth, float maxHealth)
    {
        Entity = target;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
    public EntityBase Entity;
    public float CurrentHealth;
    public float MaxHealth;
}

