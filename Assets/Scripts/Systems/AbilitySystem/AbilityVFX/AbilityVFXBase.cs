using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityVFXBase : ScriptableObject
{
    public abstract void PlayVFX(EntityBase origin, AbilityBase ability, EntityBase target = null);
    public virtual void EndVFX() { }
    public virtual void CleanUp() { }
}