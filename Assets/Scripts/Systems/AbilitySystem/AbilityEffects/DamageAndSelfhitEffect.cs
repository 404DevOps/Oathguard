using System;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class DamageAndSelfhitEffect : AbilityEffectBase
{
    public DamageEffect DamageEffect;
    [Range(0, 1)] public float SelfdamageFraction;
    public bool SelfDamageAsTrueDamage;


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
        PlayEffectVFX(origin, target);
        EntityBase tar = DamageEffect.TargetType == TargetType.Origin ? origin : target;
        if (tar == null) return;

        var tarHealth = tar.Health;
        if (tarHealth == null)
            return;
        var orHealth = origin.Health;
        if (orHealth == null)
            return;

        var dmgData = CombatSystem.Instance.CalculateDamage(origin, target, DamageEffect);
        dmgData.Type = DamageEffect.Type;
        if (!dmgData.IsImmune && dmgData.FinalDamage > 0 && !tar.Health.HasDied)
        {
            dmgData.SourceOathUpgrade = sourceOathUpgrade;
            tarHealth.ApplyDamage(dmgData);
        }

        var selfDamageFraction = SelfdamageFraction * dmgData.FinalDamage;
        if(!CombatSystem.Instance.HasImmunity(origin, DamageEffect.Type))
            origin.Health.ApplyDamage(new DamageContext(origin, origin) { FinalDamage = selfDamageFraction, IgnoreHurt = true, IsTrueDamage = SelfDamageAsTrueDamage, SourceEffect = this.DamageEffect });
    }
}

