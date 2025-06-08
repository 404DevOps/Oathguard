using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityVFXBase : ScriptableObject
{
    public abstract void PlayVFX(EntityBase origin, AbilityBase ability, EntityBase target = null, Vector3? offset = null);
    public abstract void EndVFX();

    public abstract void CleanUp();
}