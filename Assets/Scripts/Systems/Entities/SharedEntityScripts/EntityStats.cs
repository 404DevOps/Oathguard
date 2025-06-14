using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [HideInInspector]
    public ResourceType ResourceType;

    public StatMediator StatMediator => _mediator;
    protected StatMediator _mediator = new StatMediator();
    protected EntityBaseStats _baseStats;

    public virtual void Initialize(EntityBase entity)
    {
        _baseStats = EntityStatMapping.Instance().GetBaseStats(entity.Type);
        ResourceType = _baseStats.ResourceType;
    }

    public float MaxHealth
    {
        get
        {
            return _mediator.GetModifiedStat(StatType.MaxHealth, _baseStats.MaxHealth);
        }
    }
    public float MaxResource
    {
        get
        {
            return _mediator.GetModifiedStat(StatType.MaxResource, _baseStats.MaxResource);
        }
    }
    public float Attack
    {
        get
        {
            return _mediator.GetModifiedStat(StatType.Attack, _baseStats.Attack);
        }
    }
    public float Defense
    {
        get
        {
            return _mediator.GetModifiedStat(StatType.Defense, _baseStats.Defense);
        }
    }
    public float MoveSpeed
    {
        get
        {
            return _mediator.GetModifiedStat(StatType.MoveSpeed, _baseStats.MoveSpeed);
        }
    }
    public float CritChance
    {
        get
        {
            return _mediator.GetModifiedStat(StatType.CritChance, _baseStats.CritChance);
        }
    }
    public float OathModifier(OathType Oath)
    {
        var baseOathModifier = _baseStats.OathModifier.FirstOrDefault(em => em.Oath == Oath);
        var baseValue = baseOathModifier == default ? 0 : baseOathModifier.Value;

        return _mediator.GetModifiedStat(StatType.OathModifier, baseValue, Oath);
    }
    public List<StatInfo> GetAllStatsAsList()
    {
        var result = new List<StatInfo>(){
            new (StatType.MaxHealth, MaxHealth),
            new (StatType.MoveSpeed, MoveSpeed),
            new (StatType.Attack, Attack),
            new (StatType.Defense, Defense),
            new (StatType.CritChance, CritChance)
        };

        var oathValues = Enum.GetValues(typeof(OathType));
        foreach (OathType oath in oathValues)
        {
            if (oath == OathType.None)
                continue;

            var value = OathModifier(oath);
            result.Add(new StatInfo(StatType.OathModifier, value, oath));
        }

        return result;
    }
}