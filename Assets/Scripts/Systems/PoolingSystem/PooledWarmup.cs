using UnityEngine;

public static class PooledWarmup
{
    public static void Preload(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var obj = Pooled.Instantiate(prefab);
            Pooled.Release(obj);
        }
    }

    public static void Preload<T>(T prefab, int count) where T : Component
    {
        for (int i = 0; i < count; i++)
        {
            var obj = Pooled.Instantiate(prefab);
            Pooled.Release(obj);
        }
    }
}