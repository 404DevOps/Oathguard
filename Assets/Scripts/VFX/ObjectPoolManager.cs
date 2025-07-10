using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Pool;

internal class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private bool _addToDontDestroyOnLoad = false;

    public static PoolType PoolingType;

    private static GameObject _emptyHolder;
    private static GameObject _particleSystemsEmpty;
    private static GameObject _vfxEmpty;
    private static GameObject _sfxEmpty;
    private static GameObject _gameobjectsEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    private void Awake()
    {
        SetupEmpties();
        _objectPools = new();
        _cloneToPrefabMap = new();
    }

    private void SetupEmpties()
    {
        _emptyHolder = new GameObject("Holder");
        _emptyHolder.transform.SetParent(this.transform);

        _particleSystemsEmpty = new GameObject("Particle Systems");
        _particleSystemsEmpty.transform.SetParent(_emptyHolder.transform);

        _vfxEmpty = new GameObject("Visual Effects");
        _vfxEmpty.transform.SetParent(_emptyHolder.transform);
        _sfxEmpty = new GameObject("Sound Effects");
        _sfxEmpty.transform.SetParent(_emptyHolder.transform);
        _gameobjectsEmpty = new GameObject("GameObjects");
        _gameobjectsEmpty.transform.SetParent(_emptyHolder.transform);

        if (_addToDontDestroyOnLoad)
            DontDestroyOnLoad(_particleSystemsEmpty.transform.root);
    }

    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType pooltype = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new(createFunc: () => CreateObject(prefab, pos, rot, pooltype),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
            );

        _objectPools.Add(prefab, pool);

    }
    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType pooltype = PoolType.GameObjects)
    {
        GameObject obj = Instantiate(prefab, pos, rot);
        GameObject parentObject = GetParentObject(pooltype);
        obj.SetActive(true);
        obj.transform.SetParent(parentObject.transform);
        return obj;
    }

    private static GameObject GetParentObject(PoolType pooltype)
    {
        switch (pooltype)
        {
            case PoolType.VisualEffect:
                return _vfxEmpty;
            case PoolType.ParticleSystems:
                return _particleSystemsEmpty;
            case PoolType.SoundFX:
                return _sfxEmpty;
            case PoolType.GameObjects:
                return _gameobjectsEmpty;
            default:
                return null;
        }
    }

    private static void OnGetObject(GameObject obj) { }
    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }
    private static void OnDestroyObject(GameObject obj)
    {
        if (_cloneToPrefabMap.ContainsKey(obj))
            _cloneToPrefabMap.Remove(obj);
    }

    public static T SpawnObject<T>(GameObject objectToSpawn, Vector3 position, Quaternion rotation, PoolType pooltype = PoolType.GameObjects) where T : UnityEngine.Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, position, rotation, pooltype);
        }

        GameObject obj = _objectPools[objectToSpawn].Get();
        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }
            obj.transform.position = position;
            obj.transform.rotation = rotation;

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Object {objectToSpawn.name} doesnt have component of type {typeof(T)}");
            }
            return component;
        }
        return null;
    }
    public static T SpawnObject<T>(T typePrefab, Vector3 position, Quaternion rotation, PoolType pooltype = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, position, rotation, pooltype);
    }
    public static GameObject SpawnObject(GameObject objToSpawn, Vector3 position, Quaternion rotation, PoolType pooltype = PoolType.GameObjects)
    {
        return SpawnObject(objToSpawn, position, rotation, pooltype);
    }
    public static GameObject SpawnObject(GameObject objToSpawn, PoolType pooltype = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objToSpawn, Vector3.zero, Quaternion.identity, pooltype);
    }
    public static void ReturnObjectToPool(GameObject obj, PoolType type)
    {
        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = GetParentObject(type);

            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("Trying to return an object that is not pooled: " + obj.name);
        }
    }
}


