using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ApplyAuraEffect : AbilityEffectBase
{
    public AuraBase Aura;
    public override void Apply(EntityBase origin, EntityBase target, float stackMultiplier = 1)
    {
        var tar = target == null ? null : target;
        Aura.Apply(origin, tar);
    }
}