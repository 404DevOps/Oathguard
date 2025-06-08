using System;
using UnityEngine;

[Serializable]
public class DamageEntityEffect : AbilityEffectBase
{
    public TargetType TargetType;
    public SealType SealType;
    public float MinDamage;
    public float MaxDamage;
    public float ShakeIntensity;
    public bool IsTrueDamage;
    public bool CanCrit;
    public bool PreventHurt;

    public override void Apply(EntityBase origin, EntityBase target, float stackMultiplier = 1)
    {
        EntityBase tar = null;
        if (TargetType == TargetType.Origin)
            tar = origin.GetComponentInChildren<EntityBase>();
        else
            tar = target;

        if (tar == null)
        {
            Debug.LogError("AbilityEffect had an invalid TargetType");
            return;
        }

        var health = tar.Health; //.GetHealthComponent();
        if (health == null)
            return;

        var dmgData = DamageCalculator.Instance.GetCalculatedDamage(origin, target, this, ShakeIntensity);
        health.ReduceHealth(dmgData);
    }
}