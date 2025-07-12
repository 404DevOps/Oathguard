using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public GameObject Prefab { get; private set; }

    public void Init(GameObject prefab)
    {
        Prefab = prefab;
    }
}