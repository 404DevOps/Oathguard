using System;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class DamageAndSelfhitEffect : AbilityEffectBase
{
    public DamageEffect DamageEffect;
    [Range(0, 1)] public float SelfdamageFraction;

    public override void Apply(EntityBase origin, EntityBase target)
    {
        var ctx = CombatSystem.Instance.CalculateDamage(origin, target, DamageEffect);
        target.Health.ApplyDamage(ctx);

        var selfDamageFraction = SelfdamageFraction * ctx.FinalDamage;
        origin.Health.ApplyDamage(new DamageContext(origin, origin) { FinalDamage = selfDamageFraction });
    }
    public override void Apply(EntityBase origin, EntityBase target, OathUpgrade sourceOathUpgrade)
    {
        var ctx = CombatSystem.Instance.CalculateDamage(origin, target, DamageEffect);
        ctx.SourceOathUpgrade = sourceOathUpgrade;
        target.Health.ApplyDamage(ctx);

        var selfDamageFraction = SelfdamageFraction * ctx.FinalDamage;
        origin.Health.ApplyDamage(new DamageContext(origin, origin) { FinalDamage = selfDamageFraction, IgnoreHurt = true });
    }
}

