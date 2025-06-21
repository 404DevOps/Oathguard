using System;
using UnityEngine;

[Serializable]
public class DamageEffect : AbilityEffectBase
{
    public TargetType TargetType;
    public float MinDamage;
    public float MaxDamage;
    public float ShakeIntensity;
    public bool IsTrueDamage;
    public bool CanCrit;
    public bool IgnoreHurt;

    public DamageType Type;

    public override void Apply(EntityBase origin, EntityBase target)
    {
        EntityBase tar = TargetType == TargetType.Origin ? origin : target;
        if (tar == null) return;

        var health = tar.Health;
        if (health == null)
            return;

        var dmgData = CombatSystem.Instance.CalculateDamage(origin, target, this);
        dmgData.Type = Type;
        if (!dmgData.IsImmune && dmgData.FinalDamage > 0)
        {
            health.ApplyDamage(dmgData);
            HandleShake(ShakeIntensity);
        }
    }
    public override void Apply(EntityBase origin, EntityBase target, OathUpgrade sourceOathUpgrade)
    {
        EntityBase tar = TargetType == TargetType.Origin ? origin : target;
        if (tar == null) return;

        var health = tar.Health;
        if (health == null)
            return;

        var dmgData = CombatSystem.Instance.CalculateDamage(origin, target, this);
        dmgData.SourceOathUpgrade = sourceOathUpgrade;
        if (!dmgData.IsImmune && dmgData.FinalDamage > 0)
        {
            health.ApplyDamage(dmgData);
            HandleShake(ShakeIntensity);
        }
    }

    private void HandleShake(float shakeIntensity)
    {
        //shakeManager.Shake(intentsity);
        return;
    }
}