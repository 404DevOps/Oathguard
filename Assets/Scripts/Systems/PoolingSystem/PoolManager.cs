using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private readonly Dictionary<GameObject, ObjectPool<GameObject>> _pools = new();
    private readonly Dictionary<GameObject, GameObject> _instancesToPrefab = new();

    private Transform _poolRoot;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _poolRoot = new GameObject("PoolRoot (Don't Touch)").transform;
        //_poolRoot.gameObject.hideFlags = HideFlags.HideInHierarchy;
        DontDestroyOnLoad(_poolRoot.gameObject);
    }

    public GameObject Get(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent)
    {
        if (!_pools.TryGetValue(prefab, out var pool))
        {
            pool = new ObjectPool<GameObject>(
                () =>
                {
                    var go = GameObject.Instantiate(prefab);
                    go.AddComponent<PooledObject>().Init(prefab);
                    go.SetActive(true);
                    return go;
                },
                actionOnGet: go =>
                {
                    if(go == null)
                        Debug.Log("Pooled Object was destroyed: " + go.name);
                    go.SetActive(true);
                },
                actionOnRelease: go =>
                {
                    go.SetActive(false);
                    go.transform.SetParent(_poolRoot);

                    var autoReturn = go.GetComponent<PooledAutoReturn>();
                    if (autoReturn != null)
                        autoReturn.Cancel();
                },
                actionOnDestroy: GameObject.Destroy,
                collectionCheck: false, // only enable in dev
                defaultCapacity: 10
            );

            _pools[prefab] = pool;
        }

        var obj = pool.Get();
        if (pos != default || rot != default)
        {
            obj.transform.SetPositionAndRotation(pos, rot);
        }
        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }
        _instancesToPrefab[obj] = prefab;
        return obj;
    }

    public void Release(GameObject obj)
    {
        if (!obj.TryGetComponent<PooledObject>(out var pooledObj) || !_pools.TryGetValue(pooledObj.Prefab, out var pool))
        {
            Debug.LogWarning($"Trying to release an object that was not pooled: {obj.name}", obj);
            GameObject.Destroy(obj);
            return;
        }

        pool.Release(obj);
    }
}
