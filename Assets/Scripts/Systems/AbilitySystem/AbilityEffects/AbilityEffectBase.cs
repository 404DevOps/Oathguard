using System;

[Serializable]
public abstract class AbilityEffectBase
{
    public abstract void Apply(EntityBase origin, EntityBase target);
    public virtual void Apply(EntityBase origin, EntityBase target, OathUpgrade sourceOathUpgrade) 
    {
        Apply(origin, target); //use standard apply method if no override found
    }
}