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
    public bool PreventHurt;

    public DamageType Type { get; internal set; }

    public override void Apply(EntityBase origin, EntityBase target)
    {
        EntityBase tar = TargetType == TargetType.Origin ? origin : target;
        if (tar == null) return;

        var health = tar.Health;
        if (health == null)
            return;

        var dmgData = CombatSystem.Instance.CalculateDamage(origin, target, this);
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