using System.Collections.Generic;
using UnityEngine;

public class WeaponEffectManager : Singleton<WeaponEffectManager>
{
    [Header("VFXPool")]
    public int PoolSize;
    public GameObject HitEffectPrefab;

    [Header("EffectSpawning")]
    public float ImpactOffsetDistance;

    //internals
    private Queue<GameObject> _vfxPool = new();
    private Transform _vfxPoolTransform;


    private void Start()
    {
        _vfxPoolTransform = transform.Find("VFXPool");
        if (_vfxPoolTransform == null) //create if not found
        {
            var go = new GameObject("VFXPool");
            go.transform.SetParent(transform);
            _vfxPoolTransform = go.transform;
        }
        WarmupPool();
    }

    public void PlayHitVFX(EntityBase origin, EntityBase target)
    {
        if (target.IsDead) return;
        Vector3 direction = (origin.transform.position - target.transform.position).normalized;
        Vector3 impactPos = target.transform.position + direction * ImpactOffsetDistance;
        impactPos.y = 1.4f;

        var hitEffect = GetVFX();
        hitEffect.transform.position = impactPos;
        hitEffect.SetActive(true);
    }

    #region ObjectPooling
    private void WarmupPool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            ReturnToPool(CreateVFX());
        }
    }

    private GameObject CreateVFX()
    {
        var vfxInstance = Instantiate(HitEffectPrefab, _vfxPoolTransform);
        var pooledVFX = vfxInstance.GetComponent<PooledVFX>();
        pooledVFX.Init(ReturnToPool);
        return vfxInstance;
    }

    private GameObject GetVFX()
    {
        if (_vfxPool.Count > 0)
        {
            return _vfxPool.Dequeue();
        }
        else
        {
            return CreateVFX();
        }
    }

    private void ReturnToPool(GameObject vfxInstance)
    {
        vfxInstance.SetActive(false);
        _vfxPool.Enqueue(vfxInstance);
    }

    #endregion
}
