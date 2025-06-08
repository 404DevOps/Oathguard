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
    }

    public float MaxHealth
    {
        get
        {
            return _mediator.GetModifiedStat(StatType.MaxHealth, _baseStats.MaxHealth);
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
    public float SealModifier(SealType Seal)
    {
        var baseSealModifier = _baseStats.SealModifier.FirstOrDefault(em => em.Seal == Seal);
        var baseValue = baseSealModifier == default ? 0 : baseSealModifier.Value;

        return _mediator.GetModifiedStat(StatType.SealModifier, baseValue, Seal);
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

        var sealValues = Enum.GetValues(typeof(SealType));
        foreach (SealType seal in sealValues)
        {
            if (seal == SealType.None)
                continue;

            var value = SealModifier(seal);
            result.Add(new StatInfo(StatType.SealModifier, value, seal));
        }

        return result;
    }
}