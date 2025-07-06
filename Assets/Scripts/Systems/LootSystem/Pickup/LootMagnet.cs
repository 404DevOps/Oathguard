using System;
using System.Collections.Generic;
using UnityEngine;

public class LootMagnet : MonoBehaviour
{
    private List<PickupBase> lootInRange = new List<PickupBase>();
    private List<PickupBase> lootToRemove = new List<PickupBase>();
    private List<PickupBase> removeBuffer = new List<PickupBase>(); //buffer for current frame.

    void TryAddLootToTracking(Collider other)
    {
        PickupBase loot = other.GetComponent<PickupBase>();
        if (loot != null)
        {
            if (!loot.CanBeMagnetized) return;
            if (!lootInRange.Contains(loot))
            {
                lootInRange.Add(loot);
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
        lootToRemove.Add(pickup);
    }

    void Update()
    {
        // Move loot toward the player
        foreach (var loot in lootInRange)
        {
            if (loot != null)
                loot.MoveToward(transform.position);
        }

        //make snapshot to buffer, so we can safely add new pickups to lootToRemove.
        if (lootToRemove.Count > 0)
        {
            removeBuffer.Clear();
            removeBuffer.AddRange(lootToRemove);
            lootToRemove.Clear(); //new entries during this frame should be kept for next update
        }

        //remove buffer.
        foreach (var loot in removeBuffer)
        {
            lootInRange.Remove(loot);
        }
    }
}