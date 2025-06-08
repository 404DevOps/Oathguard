using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Auras/BaseAura", fileName = "BaseAura")]
public class AuraBase : UniqueScriptableObject
{
    public string Name;
    public Sprite Icon;

    public TargetType TargetType;
    public AuraType Type;

    public float Duration;
    public List<StatModifier> Modifiers;

    public bool Unique;
    public bool ShowAuraDisplay;

    protected EntityBase _appliedTo;

    public virtual void Apply(EntityBase origin, EntityBase target)
    {
        _appliedTo = TargetType == TargetType.Origin ? origin : target;

        var addedFresh = AuraManager.Instance.AddOrRefreshAura(_appliedTo.Id, this);

        if (addedFresh)
        {
            foreach (var modifier in Modifiers)
            {
                var stats = _appliedTo.GetComponent<EntityStats>();
                modifier.SourceId = Id;
                stats.StatMediator.AddModifier(modifier);
            }
        }
    }
    public virtual void Expire()
    {
        if (_appliedTo == null)
            return;

        var stats = _appliedTo.GetComponent<EntityStats>();
        stats.StatMediator.RemoveModifiersBySourceId(Id);
    }
}