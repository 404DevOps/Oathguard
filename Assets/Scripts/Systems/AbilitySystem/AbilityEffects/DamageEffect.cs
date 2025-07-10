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
    public bool AllowTriggerReactiveEvents;

    public DamageType Type;

    public override void Apply(EntityBase origin, EntityBase target, AbilityBase ability = null)
    {
        ApplyInternal(origin, target, null);
    }
    public override void Apply(EntityBase origin, EntityBase target, OathUpgrade sourceOathUpgrade)
    {
        ApplyInternal(origin, target, sourceOathUpgrade);
    }
    private void ApplyInternal(EntityBase origin, EntityBase target, OathUpgrade sourceOathUpgrade)
    {
        PlayEffectVFX(origin, target, null);
        EntityBase tar = TargetType == TargetType.Origin ? origin : target;
        if (tar == null) return;

        var health = tar.Health;
        if (health == null)
            return;

        var dmgData = CombatSystem.Instance.CalculateDamage(origin, target, this);
        dmgData.SourceOathUpgrade = sourceOathUpgrade;
        if (!dmgData.IsImmune && dmgData.FinalDamage > 0 && !tar.Health.HasDied)
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