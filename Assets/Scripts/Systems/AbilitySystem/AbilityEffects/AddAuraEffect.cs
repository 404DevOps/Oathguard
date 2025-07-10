using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddAuraEffect : AbilityEffectBase
{
    public AuraBase Aura;
    public override void Apply(EntityBase origin, EntityBase target, AbilityBase ability = null)
    {
        PlayEffectVFX(origin, target, ability);
        var tar = Aura.TargetType == TargetType.Origin ? origin : target;
        AuraManager.Instance.ApplyAura(origin, tar, Aura);
    }
}