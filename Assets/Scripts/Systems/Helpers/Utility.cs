using UnityEngine;

public static class Utility
{
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    public static void DeleteChildren(this Transform transform)
    {
        while (transform.childCount > 0) Object.DestroyImmediate(transform.GetChild(0).gameObject);
    }

    public static bool IsInLayerMask(LayerMask mask, GameObject gameobject)
    {
        return gameobject != null && ((1 << gameobject.layer) & mask.value) != 0;
    }
}
