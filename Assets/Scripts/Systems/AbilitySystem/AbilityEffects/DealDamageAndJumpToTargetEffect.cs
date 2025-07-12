using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChainLightningEffect : DamageEffect
{
    public float JumpRange = 5f;
    public int MaxJumps = 3;
    public float DamageMultiplier = 1f;
    public float LayerMask;

    public override void Apply(EntityBase origin, EntityBase initialTarget, OathUpgrade source)
    {
        if (initialTarget == null) return;

        AllowTriggerReactiveEvents = false;

        var alreadyHit = new HashSet<EntityBase> { initialTarget };
        var current = initialTarget;

        //initial target first
        base.Apply(origin, initialTarget, source);

        //do jumps
        for (int i = 0; i < MaxJumps - 1; i++)
        {
            EntityBase nextTarget = FindClosestUnit(current.transform.position, alreadyHit);
            if (nextTarget == null) break;

            base.Apply(current, nextTarget, source);
            Debug.Log("ChainTarget Hit");

            current = nextTarget;
        }
    }

    private EntityBase FindClosestUnit(Vector3 fromPos, HashSet<EntityBase> alreadyHit)
    {
        EntityBase closest = null;
        float minDistSqr = float.MaxValue;

        foreach (var enemy in EntityManager.Instance.Entities)
        {
            if (enemy is PlayerEntity) continue;
            if (enemy.IsDead) continue;
            if (enemy == null || alreadyHit.Contains(enemy))
                continue;

            float sqrDist = (enemy.transform.position - fromPos).sqrMagnitude;
            if (sqrDist <= JumpRange * JumpRange && sqrDist < minDistSqr)
            {
                closest = enemy;
                minDistSqr = sqrDist;
            }
        }

        return closest;
    }
}
