using System;

[Serializable]
public abstract class AbilityEffectBase
{
    public abstract void Apply(EntityBase origin, EntityBase target);
}