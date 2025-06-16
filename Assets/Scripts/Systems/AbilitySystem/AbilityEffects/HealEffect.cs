public class HealEffect : AbilityEffectBase
{
    public TargetType Target;
    public float MinAmount;
    public float MaxAmount;

    public bool IsTrueHealing { get; internal set; }

    public override void Apply(EntityBase origin, EntityBase target)
    {
        var tar = Target == TargetType.Origin ? origin : target;
        if (tar == null) return;
        var healContext = CombatSystem.Instance.CalculateHealing(origin, tar, this);

        if(healContext.FinalAmount > 0)
            tar.Health.ApplyHealing(healContext);
    }
}

