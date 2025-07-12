using System;
using System.Collections;
using UnityEngine;

public abstract class HitDetectionBase : ScriptableObject
{
    public float Range;
    protected float Duration;
    public abstract IEnumerator Execute(EntityBase origin, LayerMask enemyLayer, float hitDuration, Action<EntityBase, EntityBase> onHit);
}
