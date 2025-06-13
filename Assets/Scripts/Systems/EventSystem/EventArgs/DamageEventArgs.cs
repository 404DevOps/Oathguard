using System.Collections.Generic;

public struct DamageEventArgs
{
    public DamageEventArgs(EntityBase origin, EntityBase target, float damageAmount, AttackEffectivityType effectivity, bool isCrit, bool isImmune = false, OathType seal = OathType.None)
    {
        Origin = origin;
        Target = target;
        Amount = damageAmount;
        Effectivity = effectivity;
        IsCrit = isCrit;
        IsImmune = isImmune;
        Seal = seal;
    }

    public EntityBase Origin;
    public EntityBase Target;
    public float Amount;

    public OathType Seal;
    AttackEffectivityType Effectivity;
    public bool IsCrit;
    public bool IsImmune;

}