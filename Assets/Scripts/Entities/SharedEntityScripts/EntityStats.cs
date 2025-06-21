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

    public List<StatData> GetAllStatsAsList()
    {
        var result = new List<StatData>(){
            new (StatType.MaxHealth, MaxHealth),
            new (StatType.MoveSpeed, MoveSpeed),
            new (StatType.Defense, Defense),
            new (StatType.CritChance, CritChance)
        };

        return result;
    }
}