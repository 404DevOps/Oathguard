using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class OnHitVFX
{
    public GameObject VFXPrefab;
    public float Duration;
    public Vector3 Offset;

    private GameObject _fxInstance;

    public void PlayVFX(EntityBase target)
    {
        if (VFXPrefab == null)
            return;

        var pos = target.transform.position + Offset;
        _fxInstance = GameObject.Instantiate(VFXPrefab, pos, Quaternion.identity);
        CoroutineUtility.Instance.RunCoroutineTracked(DestroyAfterDuration(Duration));
    }

    private IEnumerator DestroyAfterDuration(float Duration)
    {
        yield return WaitManager.Wait(Duration);

        GameObject.Destroy(_fxInstance);
    }
}