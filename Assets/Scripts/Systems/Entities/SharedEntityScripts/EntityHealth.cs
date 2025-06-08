using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityHealth : MonoBehaviour
{
    EntityConfiguration _config;
    public bool IsHurt = false;
    public float MaxHealth => EntityStats.MaxHealth;
    public float CurrentHealth;
    public EntityStats EntityStats;
    public Action EntityDied;

    public float HealthAsPercentage { get { return CurrentHealth / MaxHealth * 100; } }

    public bool IsInvincible;

    #region Bodypart Logic 

    private EntityBase _entity;

    #endregion

    public void Initialize(EntityStats stats)
    {
        EntityStats = stats;
        CurrentHealth = MaxHealth;
        _config = stats.GetComponent<EntityConfiguration>();
        _entity = GetComponent<EntityBase>();
    }

    public void ReduceHealth(DamageEventArgs damageEventData)
    {
        if (CurrentHealth <= 0) //already dead
            return;

        if (IsInvincible)
        {
            damageEventData.IsImmune = true;
            GameEvents.OnEntityDamaged.Invoke(damageEventData);
            return;
        }


            CurrentHealth -= damageEventData.Amount;

            if (damageEventData.Amount > 0)
            {
                if (_config.HurtDuration > 0)
                {
                    IsHurt = true;
                    StartCoroutine(StopHurt()); //allow for custom hurt duration
                }
            }
            if (CurrentHealth <= 0)
            {
                GameEvents.OnEntityDied.Invoke(_entity.Id);
                EntityDied?.Invoke();
            }
        

        GameEvents.OnEntityHurt.Invoke(_entity.Id);
        GameEvents.OnEntityDamaged.Invoke(damageEventData);
        GameEvents.OnEntityHealthChanged.Invoke(new HealthChangedEventArgs(damageEventData.Target, CurrentHealth, MaxHealth));
    }

    private IEnumerator StopHurt(float hurtDuration = 0)
    {
        yield return new WaitForSeconds(hurtDuration > 0 ? hurtDuration : _config.HurtDuration);
        IsHurt = false;
    }

    public void AddHealth(float amount)
    {
        CurrentHealth += amount;

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        var entity = GetComponent<EntityBase>();
        GameEvents.OnEntityHealed.Invoke(new HealEventArgs(entity, amount));
        GameEvents.OnEntityHealthChanged.Invoke(new HealthChangedEventArgs(entity, MaxHealth, CurrentHealth));
    }

    public void SetInvincible(bool isInvincible)
    {
        IsInvincible = isInvincible;
    }
}