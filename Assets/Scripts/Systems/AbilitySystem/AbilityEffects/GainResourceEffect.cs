using System;

public class GainResourceEffect : AbilityEffectBase
{
    public TargetType Target;
    public float MinAmount;
    public float MaxAmount;

    public override void Apply(EntityBase origin, EntityBase target)
    {
        var tar = Target == TargetType.Origin ? origin : target;
        if (tar == null) return;
        var resourceAmount = UnityEngine.Random.Range(MinAmount, MaxAmount);

        tar.Resource.ChangeResource(resourceAmount);
    }
}

