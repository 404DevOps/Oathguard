using System;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public float MaxHealth => EntityStats.MaxHealth;
    public float CurrentHealth;

    //references
    public EntityStats EntityStats;
    public EntityBase Entity;

    public float HealthPercentage { get { return CurrentHealth / MaxHealth * 100; } }

    public void Initialize(EntityBase entity)
    {
        EntityStats = entity.Stats;
        CurrentHealth = MaxHealth;

        Entity = entity;
    }

    public void ApplyDamage(DamageContext damageData)
    {
        if (CurrentHealth <= 0)
            return;

        CurrentHealth -= damageData.FinalDamage;
        CurrentHealth = Mathf.Max(0, CurrentHealth); //clamp

        GameEvents.OnEntityDamageReceived.Invoke(damageData);
        GameEvents.OnEntityHealthChanged.Invoke(new HealthChangedEventArgs(damageData.Target, CurrentHealth, MaxHealth));

        if (CurrentHealth <= 0)
        {
            GameEvents.OnEntityDied.Invoke(Entity.Id);
        }
    }

    public void ApplyHealing(HealingContext healData)
    {
        CurrentHealth += healData.FinalAmount;

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        GameEvents.OnEntityHealed.Invoke(healData);
        GameEvents.OnEntityHealthChanged.Invoke(new HealthChangedEventArgs(Entity, CurrentHealth, MaxHealth));
    }
}