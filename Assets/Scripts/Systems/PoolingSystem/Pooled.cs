using UnityEngine;

public static class Pooled
{
    private static PoolManager _manager;

    [RuntimeInitializeOnLoadMethod]
    private static void Init() => _manager ??= PoolManager.Instance;

    public static T Instantiate<T>(
        T prefab,
        Vector3 pos = default,
        Quaternion rot = default,
        Transform parent = null,
        bool autoReturn = false,
        float lifetime = 2f
    ) where T : Component
    {
        var obj = _manager.Get(prefab.gameObject, pos, rot, parent);
        var component = obj.GetComponent<T>();

        if (autoReturn)
        {
            var auto = obj.GetComponent<PooledAutoReturn>();
            if (auto == null)
                auto = obj.AddComponent<PooledAutoReturn>();
            auto.Begin(lifetime);
        }

        return component;
    }

    public static GameObject Instantiate(
        GameObject prefab,
        Vector3 pos = default,
        Quaternion rot = default,
        Transform parent = null,
        bool autoReturn = false,
        float lifetime = 2f
    )
    {
        var obj = _manager.Get(prefab, pos, rot, parent);

        if (autoReturn)
        {
            var auto = obj.GetComponent<PooledAutoReturn>();
            if (auto == null)
                auto = obj.AddComponent<PooledAutoReturn>();
            auto.Begin(lifetime);
        }

        return obj;
    }

    public static void Release(GameObject obj) => _manager.Release(obj);
    public static void Release(Component comp) => Release(comp.gameObject);
}
