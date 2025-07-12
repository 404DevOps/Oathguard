using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "Ability/VFX/ElectricArcVFX")]
internal class ElectricArcVFX : AbilityVFXBase
{
    public GameObject ArcPrefab;
    public float Duration;
    public float MaxJitter;
    private VisualEffect _vfx;

    public Vector3 targetOffset;
    public Vector3 originOffset;

    public override void PlayVFX(EntityBase origin, AbilityBase ability, EntityBase target = null, float duration = 0)
    {
        if (target == null) Debug.LogError("Target must be set for ElectricArcVFX");
        Duration = Duration >= duration ? Duration : duration;
        if(ability != null)
            CoroutineUtility.Instance.RunAbilityCoroutine(PlayVFXRoutine(origin, target), ability.Id);
        else
            CoroutineUtility.Instance.RunCoroutineTracked(PlayVFXRoutine(origin, target));
    }

    private IEnumerator PlayVFXRoutine(EntityBase origin, EntityBase target)
    {
        var startPos = origin.transform.position + originOffset;
        var endPos = target.transform.position + targetOffset;

        var instance = Pooled.Instantiate(ArcPrefab);
        _vfx = instance.GetComponentInChildren<VisualEffect>();
        // set positions
        var direction = (endPos - startPos).normalized;
        float totalLength = Vector3.Distance(startPos, endPos);

        // mid points 25% from start/end
        float segmentFraction = 0.25f;
        float offsetDistance = totalLength * segmentFraction;

        var mid1 = startPos + direction * offsetDistance;
        var mid2 = endPos - direction * offsetDistance;

        // Add randomization for mid points
        Vector3 jitter = Vector3.Cross(direction, Vector3.up).normalized;
        if (jitter == Vector3.zero)
            jitter = Vector3.Cross(direction, Vector3.right).normalized;

        mid1 += jitter * UnityEngine.Random.Range(-MaxJitter, MaxJitter);
        mid2 += jitter * UnityEngine.Random.Range(-MaxJitter, MaxJitter);

        var pos1 = instance.transform.Find("Pos1");
        var pos2 = instance.transform.Find("Pos2");
        var pos3 = instance.transform.Find("Pos3");
        var pos4 = instance.transform.Find("Pos4");

        pos1.position = startPos;
        pos2.position = mid1;
        pos3.position = mid2;
        pos4.position = endPos;

        pos1.SetParent(origin.transform);
        pos4.SetParent(target.transform);

        yield return WaitManager.Wait(Duration);

        pos1.SetParent(instance.transform);
        pos4.SetParent(instance.transform);

        Pooled.Release(instance);
    }
}

