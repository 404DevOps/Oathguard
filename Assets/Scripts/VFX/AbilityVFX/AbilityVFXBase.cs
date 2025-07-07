using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityVFXBase : ScriptableObject
{
    public abstract void PlayVFX(EntityBase origin, AbilityBase ability, EntityBase target = null, float duration = 0);
    public virtual void EndVFX() { }
    public virtual void CleanUp() { }
}