using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot/WeightedLootTable")]
public class WeightedLootTable : ScriptableObject
{
    [SerializeField] private List<DropCountOption> dropCountWeights = new();
    [SerializeField] private List<LootEntry> lootEntries = new();

    private float totalWeight;
    private System.Random rng = new();

    public void RecalculateWeights()
    {
        totalWeight = 0f;
        foreach (var entry in lootEntries)
        {
            totalWeight += entry.Weight;
        }
    }

    public void SpawnLoot(Vector3 centerPosition, float scatterRadius = 1f)
    {
        RecalculateWeights();

        int dropCount = RollDropCount();

        if (dropCount == 0) return;

        for (int i = 0; i < dropCount; i++)
        {
            float roll = (float)(rng.NextDouble() * totalWeight);
            float cumulative = 0f;

            foreach (var entry in lootEntries)
            {
                cumulative += entry.Weight;
                if (roll <= cumulative)
                {
                    Vector3 offset = UnityEngine.Random.insideUnitSphere * scatterRadius;
                    offset.y = 0f;
                    entry.Drop.Spawn(centerPosition + offset);
                    break;
                }
            }
        }
    }

    private int RollDropCount()
    {
        float total = 0f;
        foreach (var option in dropCountWeights)
            total += option.Weight;

        float roll = (float)(rng.NextDouble() * total);
        float cumulative = 0f;

        foreach (var option in dropCountWeights)
        {
            cumulative += option.Weight;
            if (roll <= cumulative)
                return option.DropCount;
        }

        //assume no drop if none was rolled.
        return 0;
    }
}

[Serializable]
public class LootEntry
{
    public LootDrop Drop;
    public float Weight;

    public LootEntry(LootDrop drop, float weight)
    {
        Drop = drop;
        Weight = weight;
    }
}
[Serializable]
public class DropCountOption
{
    public int DropCount = 1;
    public float Weight = 1f;
}