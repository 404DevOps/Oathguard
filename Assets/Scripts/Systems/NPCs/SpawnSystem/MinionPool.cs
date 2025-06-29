using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class MinionPool
{
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly Queue<GameObject> pool;

    public MinionPool(GameObject prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        pool = new Queue<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            var obj = Object.Instantiate(prefab, parent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public NPCEntity Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Object.Instantiate(prefab, parent);
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        var entity = obj.GetComponent<NPCEntity>();
        entity?.OnSpawned();
        return entity;
    }

    public void Despawn(GameObject obj)
    {
        obj.GetComponent<NPCEntity>()?.OnDespawn();
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
