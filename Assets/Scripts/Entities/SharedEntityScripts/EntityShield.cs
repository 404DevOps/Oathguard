using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityShield : MonoBehaviour
{
    public float CurrentShield => GetTotalShield();

    private List<ShieldSource> sources = new List<ShieldSource>();

    public EntityStats EntityStats;
    public EntityBase Entity;

    public void Initialize(EntityBase entity)
    {
        Entity = entity;
        EntityStats = entity.Stats;
        sources.Clear();
    }

    public float GetTotalShield()
    {
        float total = 0f;
        foreach (var source in sources)
            total += source.Amount;
        return total;
    }

    public void SetShield(string sourceId, SourceType sourceType, float amount)
    {
        var source = sources.Find(s => s.SourceId == sourceId);
        if (source != null)
        {
            source.Amount = amount;
        }
        else
        {
            sources.Add(new ShieldSource(sourceId, sourceType, amount));
        }

        GameEvents.OnEntityShieldChanged?.Invoke(new ShieldChangedEventArgs(Entity, GetTotalShield()));
    }

    public void ReduceShield(float amount)
    {
        //reduce shields FIFO
        for (int i = 0; i < sources.Count && amount > 0; i++)
        {
            var source = sources[i];
            if (source.Amount <= amount)
            {
                amount -= source.Amount;
                source.Amount = 0;
            }
            else
            {
                source.Amount -= amount;
                amount = 0;
            }
        }
        var depletedShieldSources = sources.Where(s => s.Amount <= 0).ToList();
        foreach (var shield in depletedShieldSources)
        {
            if (shield.SourceType == SourceType.Aura)
                AuraManager.Instance.CancelAuraById(Entity.Id, shield.SourceId);
        }
        sources.RemoveAll(s => s.Amount <= 0);
        GameEvents.OnEntityShieldChanged?.Invoke(new ShieldChangedEventArgs(Entity, GetTotalShield()));
    }

    public float GetSourceAmount(string sourceId)
    {
        var source = sources.Find(s => s.SourceId == sourceId);
        return source != null ? source.Amount : 0f;
    }

    public void RemoveSource(string sourceId) //should be called from source
    {
        sources.RemoveAll(s => s.SourceId == sourceId);
        GameEvents.OnEntityShieldChanged?.Invoke(new ShieldChangedEventArgs(Entity, GetTotalShield()));
    }
}

public class ShieldSource
{
    public string SourceId;
    public SourceType SourceType;
    public float Amount;

    public ShieldSource(string sourceId, SourceType sourceType, float amount)
    {
        SourceId = sourceId;
        SourceType = sourceType;
        Amount = amount;
    }
}
