using System;
using UnityEditor.Compilation;

[Serializable]
public abstract class AbilityEffectBase
{
    public AbilityVFXBase VFX;
    
    protected virtual void PlayEffectVFX(EntityBase origin, EntityBase target, AbilityBase ability = null)
    {
        if(VFX != null)
            VFX.PlayVFX(origin, ability, target);
    }
    public abstract void Apply(EntityBase origin, EntityBase target, AbilityBase ability = null);
    public virtual void Apply(EntityBase origin, EntityBase target, OathUpgrade sourceOathUpgrade) 
    {
        Apply(origin, target); //use standard apply method if no override
    }
}