using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public float DespawnDelay;

    [System.Serializable]
    public class MinionConfig
    {
        public EntityType Type;
        public GameObject Prefab;
        public int InitialPoolSize = 10;
    }

    [SerializeField] private List<MinionConfig> minionTypes;
    private Dictionary<EntityType, MinionPool> pools;


    private void Awake()
    {
        pools = new Dictionary<EntityType, MinionPool>();
        foreach (var type in minionTypes)
        {
            if (type.Prefab == null || type.Type == EntityType.Player || type.Type == EntityType.None)
            {
                Debug.LogWarning("Invalid minion type setup.");
                continue;
            }

            pools[type.Type] = new MinionPool(type.Prefab, type.InitialPoolSize, this.transform);
        }
    }

    public NPCEntity Spawn(EntityType entityType, Vector3 position, Quaternion rotation)
    {
        if (!pools.TryGetValue(entityType, out var pool))
        {
            Debug.LogWarning($"No pool found for minion ID: {entityType}");
            return null;
        }

        return pool.Spawn(position, rotation);
    }

    public void Despawn(EntityType entityType, GameObject minion)
    {
        if (!pools.TryGetValue(entityType, out var pool))
        {
            Debug.LogWarning($"No pool found to despawn minion ID: {entityType}");
            return;
        }

        StartCoroutine(DespawnAfterDelay(pool, minion));
    }

    private IEnumerator DespawnAfterDelay(MinionPool pool, GameObject minion)
    {
        yield return WaitManager.Wait(DespawnDelay);
        pool.Despawn(minion);
    }
}
