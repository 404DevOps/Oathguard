using System;
using UnityEngine;

public class EntityShield : MonoBehaviour
{
    public float CurrentShield;

    //references
    public EntityStats EntityStats;
    public EntityBase Entity;

    public void Initialize(EntityBase entity)
    {
        Entity = entity;
        EntityStats = entity.Stats;
        CurrentShield = 0;
    }

    public float GetShieldAmount()
    {
        return CurrentShield;
    }

    public void AddShield(float amount)
    {
        CurrentShield += amount;
    }
    public void ReduceShield(float amount)
    {
        CurrentShield -= amount;
        if (CurrentShield <= 0)
            CurrentShield = 0;
    }

    internal void SetShieldAmount(float remainingShield)
    {
        CurrentShield = remainingShield;
    }
}