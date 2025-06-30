using System;
using UnityEngine;

public class LootSpawner : Singleton<LootSpawner>
{
    private void OnEnable()
    {
        GameEvents.OnEntityDied.AddListener(OnEntityDied);
    }
    private void OnDisable()
    {
        GameEvents.OnEntityDied.RemoveListener(OnEntityDied);
    }

    private void OnEntityDied(EntityBase entity)
    {
        if (entity is PlayerEntity) return;

        var lootTable = LootMapping.Instance().GetLootTable(entity.Type);
        SpawnLoot(lootTable, entity.transform.position);
    }

    public static void SpawnLoot(WeightedLootTable table, Vector3 position, float scatterRadius = 2f)
    {
        if (table == null) return;
        table.SpawnLoot(position, scatterRadius);
        // Optionally add particle effects, sound, etc.
    }
}