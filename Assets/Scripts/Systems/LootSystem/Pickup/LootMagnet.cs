using System;
using System.Collections.Generic;
using UnityEngine;

public class LootMagnet : MonoBehaviour
{
    private List<PickupBase> _lootInRange = new List<PickupBase>();
    private List<PickupBase> _lootToRemove = new List<PickupBase>();
    private List<PickupBase> _removeBuffer = new List<PickupBase>(); //buffer for current frame.

    private PlayerEntity _player;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerEntity>();
    }

    private void OnEnable()
    {
        GameEvents.OnEntityDied.AddListener(OnPlayerDied);
    }
    private void OnDisable()
    {
        GameEvents.OnEntityDied.RemoveListener(OnPlayerDied);
    }

    private void OnPlayerDied(EntityBase entity)
    {
        //release pending loot when dying
        if (entity.Id != _player.Id) return;
        foreach (var loot in _lootInRange)
        {
            Pooled.Release(loot);
        }
        foreach (var loot in _lootToRemove)
        {
            Pooled.Release(loot);
        }
        foreach (var loot in _removeBuffer)
        {
            Pooled.Release(loot);
        }

        _lootInRange.Clear();
        _lootToRemove.Clear();
        _removeBuffer.Clear();
    }

    void TryAddLootToTracking(Collider other)
    {
        PickupBase loot = other.GetComponent<PickupBase>();
        if (loot != null)
        {
            if (!loot.CanBeMagnetized) return;
            if (!_lootInRange.Contains(loot))
            {
                _lootInRange.Add(loot);
                loot.OnLootCollected += RemoveFromList;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        TryAddLootToTracking(other);
    }
    private void OnTriggerStay(Collider other)
    {
        TryAddLootToTracking(other);
    }

    private void RemoveFromList(PickupBase pickup)
    {
        _lootToRemove.Add(pickup);
    }

    void Update()
    {
        if (_player.IsDead) return;
        // Move loot toward the player
        foreach (var loot in _lootInRange)
        {
            if (loot != null)
                loot.MoveToward(transform.position);
        }

        //make snapshot to buffer, so we can safely add new pickups to lootToRemove.
        if (_lootToRemove.Count > 0)
        {
            _removeBuffer.Clear();
            _removeBuffer.AddRange(_lootToRemove);
            _lootToRemove.Clear(); //new entries during this frame should be kept for next update
        }

        //remove buffer.
        foreach (var loot in _removeBuffer)
        {
            _lootInRange.Remove(loot);
        }
    }
}