using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class DamageIfAuraActiveEffect : DamageEffect
{
    public AuraBase ConditionAura;

    public override void Apply(EntityBase origin, EntityBase target)
    {
        ApplyInternal(origin, target, null);
    }
    public override void Apply(EntityBase origin, EntityBase target, OathUpgrade sourceOathUpgrade)
    {
        ApplyInternal(origin, target, sourceOathUpgrade);
    }
    private void ApplyInternal(EntityBase origin, EntityBase target, OathUpgrade sourceOathUpgrade)
    {
        EntityBase tar = TargetType == TargetType.Origin ? origin : target;
        if (tar == null) return;

        var targetAuras = AuraManager.Instance.GetEntityAuras(tar.Id);
        if (!targetAuras.Any(a => a.Template.Id == ConditionAura.Id)) return;

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