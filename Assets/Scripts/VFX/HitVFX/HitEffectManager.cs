using System.Collections.Generic;
using UnityEngine;

public class HitEffectManager : Singleton<HitEffectManager>
{
    [Header("HitEffect")]
    public int HitEffectPoolSize;
    public GameObject HitEffectPrefab;

    [Header("SplashEffect")]
    public int SplashEffectPoolSize;
    public GameObject SplashEffectPrefab;

    [Header("EffectSpawning")]
    public float ImpactOffsetDistance;
    public float SplashOffsetDistance;


    private void Start()
    {
        PooledWarmup.Preload(HitEffectPrefab, HitEffectPoolSize);
        PooledWarmup.Preload(SplashEffectPrefab, SplashEffectPoolSize);
    }

    public void PlayHitVFX(EntityBase origin, EntityBase target)
    {
        if (target.IsDead) return;
        Vector3 direction = (origin.transform.position - target.transform.position).normalized;
        Vector3 impactPos = target.transform.position + direction * ImpactOffsetDistance;
        impactPos.y = 1.4f;

        Pooled.Instantiate(HitEffectPrefab, position: impactPos, autoReturn: true, lifetime: 2f);
    }
    public void PlaySplashVFX(EntityBase origin, EntityBase target)
    {
        if (target.IsDead) return;
        Vector3 direction = (origin.transform.position - target.transform.position).normalized;
        Vector3 impactPos = target.transform.position - direction * SplashOffsetDistance;
        impactPos.y = 1.4f;

        Quaternion rotation = Quaternion.LookRotation(-direction);

        Pooled.Instantiate(SplashEffectPrefab, position: impactPos, rotation: rotation, autoReturn: true, lifetime: 2f);
    }
}
