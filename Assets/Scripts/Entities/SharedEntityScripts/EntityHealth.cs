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

    private bool _hasDied;
    public bool HasDied => _hasDied; //this is more instantaneous than the event, prevent further calls to apply damage

    public void Initialize(EntityBase entity)
    {
        EntityStats = entity.Stats;
        CurrentHealth = MaxHealth;
        _hasDied = false;
        Entity = entity;
    }

    public void ApplyDamage(DamageContext damageData)
    {
        if (Entity.IsDead) return;

        float damageLeft = damageData.FinalDamage;
        float absorbedByShield = 0f;

        //apply shield before health
        var shield = Entity.Shield;
        if (shield != null && shield.CurrentShield > 0)
        {
            float before = shield.CurrentShield;
            shield.ReduceShield(damageLeft);
            absorbedByShield = before - shield.CurrentShield;
            damageLeft -= absorbedByShield;

            if (absorbedByShield > 0)
            {
                GameEvents.OnEntityShieldAbsorbed.Invoke(new ShieldAbsorbedEventArgs(
                    damageData.Target,
                    absorbedByShield
                ));
            }
        }

        if (damageLeft > 0 && !Entity.IsDead) //isDead will be triggered after damage that killed entity was processed, no need to go here again when already dead
        {
            CurrentHealth -= damageLeft;
            CurrentHealth = Mathf.Max(0, CurrentHealth);

            GameEvents.OnEntityDamageReceived.Invoke(damageData);
            GameEvents.OnEntityHealthChanged.Invoke(new HealthChangedEventArgs(damageData.Target, CurrentHealth, MaxHealth));

            if (CurrentHealth <= 0 && !_hasDied)
            {
                _hasDied = true;
                Entity.IsDead = true;
                GameEvents.OnEntityDied.Invoke(Entity);
            }
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