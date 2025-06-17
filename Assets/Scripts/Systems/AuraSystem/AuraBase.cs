using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Aura/BaseAura", fileName = "BaseAura")]
public class AuraBase : UniqueScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;

    public TargetType TargetType;
    public AuraType Type;

    public float Duration;
    public List<StatModifier> Modifiers;

    public bool ShowAuraDisplay;

    public virtual void OnApply(AuraInstance instance)
    {
        foreach (var modifier in Modifiers)
        {
            modifier.SourceId = Id;
            instance.Target.Stats.StatMediator.AddModifier(modifier);
        }
    }
    public virtual void OnExpire(AuraInstance instance)
    {
        instance.Target.Stats.StatMediator.RemoveModifiersBySourceId(Id);
    }

    public virtual void OnTick(AuraInstance instance)
    { 
    
    }

    internal virtual void OnRefresh(AuraInstance auraInstance)
    {
        
    }
}