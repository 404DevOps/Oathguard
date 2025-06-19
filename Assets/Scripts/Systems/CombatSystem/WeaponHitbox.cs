using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public EntityBase Holder;
    public Collider Hitbox;
    public Action<EntityBase, EntityBase> OnHit;
    public LayerMask HitLayer;
    public WeaponSet Data;

    private HashSet<EntityBase> alreadyHit = new();

    public void Initialize(EntityBase entity, WeaponSet weaponData)
    {
        Holder = entity;
        Hitbox.enabled = false;
        Hitbox.includeLayers = HitLayer;
        Data = weaponData;
    }

    public void EnableHitbox()
    {
        alreadyHit.Clear();
        Hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        Hitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Utility.IsInLayerMask(HitLayer, other.gameObject)) return;

        if (other.transform.parent.TryGetComponent(out EntityBase target))
        {
            if (target == Holder) return;
            if (alreadyHit.Contains(target)) return;
            alreadyHit.Add(target);
            Debug.Log("Hit: " + target.name);
            OnHit?.Invoke(Holder, target);
        }
    }
}