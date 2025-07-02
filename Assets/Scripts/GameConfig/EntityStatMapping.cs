using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(menuName = "Config/Entity Stat Mapping", fileName = "NewEntityStatMapping")]
internal class EntityStatMapping : ScriptableObject
{
    [SerializeField] private List<EntityStatPair> _entityStatPairs;

    private static EntityStatMapping _instance;

    public static EntityStatMapping Instance()
    {
        if (_instance == null)
        {
            _instance = Resources.Load("GameConfig/" + typeof(EntityStatMapping).Name) as EntityStatMapping;
        }
        return _instance;
    }

    public EntityBaseStats GetBaseStats(EntityType entityType)
    {
        try
        {
            return _entityStatPairs.FirstOrDefault(pair => pair.Type == entityType).BaseStats;
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not retreive BaseStats for Class {entityType}:" + e.Message);
            return null;
        }
    }
}

[Serializable]
public class EntityStatPair
{
    public EntityType Type;
    public EntityBaseStats BaseStats;
}