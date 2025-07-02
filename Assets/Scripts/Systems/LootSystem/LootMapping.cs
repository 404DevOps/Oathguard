using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Loot Mapping", fileName = "NewLootMapping")]
internal class LootMapping : ScriptableObject
{
    [SerializeField] private List<EntityLootPair> _entityLootPairs;

    private static LootMapping _instance;

    public static LootMapping Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("GameConfig/" + typeof(LootMapping).Name) as LootMapping;
        }
        return _instance;
    }
    public WeightedLootTable GetLootTable(EntityType type)
    {
        return _entityLootPairs.Find(pair => pair.EntityType == type).DropTable;
    }
}

[Serializable]
public class EntityLootPair
{ 
    public EntityType EntityType;
    public WeightedLootTable DropTable;
}
