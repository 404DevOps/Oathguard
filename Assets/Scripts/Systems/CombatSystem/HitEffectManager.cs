using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectManager : Singleton<HitEffectManager>
{
    public float ImpactOffsetDistance;
    public GameObject basicHitEffect;
    public List<HitEffectDamageTypePair> _damageTypes;

    public void InstantiateHitEffect(EntityBase origin, EntityBase target)
    {
        Vector3 direction = (origin.transform.position - target.transform.position).normalized;
        Vector3 impactPos = target.transform.position + direction * ImpactOffsetDistance;
        impactPos.y = 1.4f;

        Instantiate(basicHitEffect, impactPos, Quaternion.identity, null);
    }
}

[Serializable]
public class HitEffectDamageTypePair
{
    public DamageType DamageType;
    public GameObject HitEffect;
}
