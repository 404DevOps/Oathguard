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

        GameObject spawned = Instantiate(pickupPrefab, position, Quaternion.identity);

        if (!spawned.TryGetComponent<PickupBase>(out _))
        {
            Debug.LogError("Spawned pickup does not have a PickupBase component!");
        }
    }
}