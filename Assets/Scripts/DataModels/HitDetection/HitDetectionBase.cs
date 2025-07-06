using System;
using System.Collections;
using UnityEngine;

public abstract class HitDetectionBase : ScriptableObject
{
    public float Range;
    public float Duration;
    public abstract IEnumerator Execute(EntityBase origin, LayerMask enemyLayer, Action<EntityBase, EntityBase> onHit);
}
