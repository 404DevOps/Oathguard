using System;
using UnityEngine;

[Serializable]
public class DamageEntityEffect : AbilityEffectBase
{
    public TargetType TargetType;
    public OathType SealType;
    public float MinDamage;
    public float MaxDamage;
    public float ShakeIntensity;
    public bool IsTrueDamage;
    public bool CanCrit;
    public bool PreventHurt;

    public override void Apply(EntityBase origin, EntityBase target)
    {
        EntityBase tar = TargetType == TargetType.Origin ? origin : target;
        if (tar == null)
        {
            Debug.LogError("AbilityEffect Target was null or had invalid TargetType");
            return;
        }

        var health = tar.Health; //.GetHealthComponent();
        if (health == null)
            return;

        var dmgData = DamageCalculator.Instance.GetCalculatedDamage(origin, target, this);
        if (!dmgData.IsImmune)
            HandleShake(ShakeIntensity);

        health.ReduceHealth(dmgData);
    }

    private void HandleShake(float shakeIntensity)
    {
        //shakeManager.Shake(intentsity);
        return;
    }
}