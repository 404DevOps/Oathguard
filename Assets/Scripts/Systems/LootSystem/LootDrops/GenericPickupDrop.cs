using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot/GenericPickupDrop")]
public class GenericPickupDrop : LootDrop
{
    public GameObject pickupPrefab; // Must have a PickupBase component

    public override void Spawn(Vector3 position)
    {
        if (pickupPrefab == null)
        {
            Debug.LogWarning("Pickup prefab not assigned.");
            return;
        }

        GameObject spawned = Pooled.Instantiate(pickupPrefab, position, Quaternion.identity);

        if (spawned.TryGetComponent(out PickupBase pickup))
        {
            pickup.OnLootCollected += ReleaseOnCollected; 

        }
        else 
        {
            Debug.LogError("Spawned pickup does not have a PickupBase component!");
        }
    }

    private void ReleaseOnCollected(PickupBase pickup)
    {
        pickup.OnLootCollected -= ReleaseOnCollected;
        Pooled.Release(pickup);
    }
}