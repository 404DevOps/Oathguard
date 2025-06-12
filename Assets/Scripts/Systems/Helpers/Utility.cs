using System.Collections;
using System.Collections.Generic;
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

    private static WeaponHitbox _sword;
    public static WeaponHitbox Sword
    {
        get
        {
            if (_sword == null) 
                _sword = Object.FindAnyObjectByType<WeaponHitbox>();
            if (_sword == null)
                Debug.LogError("No Sword in Scene");

            return _sword;
        }
    }

public static void DeleteChildren(this Transform t)
    {
        while (t.childCount > 0) Object.DestroyImmediate(t.GetChild(0).gameObject);
    }

    public static Vector3 GetPosWithRandomDeviation(this Vector3 originalPos, float maxDeviation)
    {
        var pos = originalPos;
        var deviation = UnityEngine.Random.Range(-maxDeviation, maxDeviation);

        if (UnityEngine.Random.value > 0.5f)
            pos.x += deviation;
        else
            pos.y += deviation;

        return pos;
    }

    public static bool IsInLayerMask(LayerMask mask, EntityBase hit)
    {
        return hit != null && ((1 << hit.gameObject.layer) & mask.value) != 0;
    }
    public static bool IsInLayerMask(LayerMask mask, GameObject hit)
    {
        return hit != null && ((1 << hit.layer) & mask.value) != 0;
    }
}
