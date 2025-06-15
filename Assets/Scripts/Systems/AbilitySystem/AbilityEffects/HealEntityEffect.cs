using System;


internal class HealEntityEffect : AbilityEffectBase
{
    public TargetType Target;
    public float MinAmount;
    public float MaxAmount;


    public override void Apply(EntityBase origin, EntityBase target)
    {
        var tar = Target == TargetType.Origin ? origin : target;
        tar.Health.AddHealth(origin, target, DamageCalculator.GetRoll(MinAmount, MaxAmount));
    }
}

