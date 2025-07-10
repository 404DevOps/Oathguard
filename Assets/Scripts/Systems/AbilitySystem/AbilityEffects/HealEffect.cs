public class HealEffect : AbilityEffectBase
{
    public TargetType Target;
    public float MinAmount;
    public float MaxAmount;

    public bool IsTrueHealing;
    public bool CanCrit;

    public override void Apply(EntityBase origin, EntityBase target, AbilityBase ability = null)
    {
        PlayEffectVFX(origin, target, null);
        var tar = Target == TargetType.Origin ? origin : target;
        if (tar == null) return;
        var healContext = CombatSystem.Instance.CalculateHealing(origin, tar, this);

        if(healContext.FinalAmount > 0)
            tar.Health.ApplyHealing(healContext);
    }
}

