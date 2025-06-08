
public struct HealthChangedEventArgs
{
    public HealthChangedEventArgs(EntityBase target, float currentHealth, float maxHealth)
    {
        Target = target;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }
    public EntityBase Target;
    public float CurrentHealth;
    public float MaxHealth;
}

